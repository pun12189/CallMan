using BahiKitab.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BahiKitab.Services
{
    public class PurchaseDataService : BaseDataService
    {
        // 1. Check if invoice already exists for a vendor
        public async Task<int?> GetExistingpurchaseId(int vendorId, string invoiceNo)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteScalarAsync<int?>(
            "SELECT Id FROM purchases WHERE vendorId=@vId AND invoiceNumber=@inv",
            new { vId = vendorId, inv = invoiceNo });
            }                     
        }

        // 2. The Master Save/Update Transaction
        public async Task<bool> SaveFullPurchase(PurchaseHeader header, List<PurchaseItem> items)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                using (var trans = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        int? existingId = await GetExistingpurchaseId(header.VendorId, header.InvoiceNo);

                        if (existingId.HasValue)
                        {
                            // UPDATE MODE: Reverse old stock and clear old rows
                            await connection.ExecuteAsync(@"UPDATE inventory p 
                                         JOIN purchaseDetails pd ON p.Id = pd.productId 
                                         SET p.stock = p.stock - pd.Qty 
                                         WHERE pd.purchaseId = @pId", new { pId = existingId }, trans);

                            await connection.ExecuteAsync("DELETE FROM purchaseDetails WHERE purchaseId = @pId", new { pId = existingId }, trans);

                            header.Id = existingId.Value;
                            await connection.ExecuteAsync("UPDATE purchases SET totalAmount=@total WHERE id=@Id", header, trans);
                        }
                        else
                        {
                            // INSERT MODE: New Header
                            string sql = @"INSERT INTO purchases (vendorId, invoiceNumber, invoiceDate, originalInvNo, totalAmount) 
                               VALUES (@VendorId, @InvoiceNo, @InvoiceDate, @OriginalInvNo, @TotalAmount);
                               SELECT LAST_INSERT_ID();";
                            header.Id = await connection.QuerySingleAsync<int>(sql, header, trans);
                        }

                        // 3. Insert new items and update current stock levels
                        foreach (var item in items)
                        {
                            await connection.ExecuteAsync(@"INSERT INTO purchaseDetails (purchaseId, productId, qty, basePrice, gstRate) 
                                         VALUES (@pId, @prodId, @qty, @price, @tax)",
                                                     new { pId = header.Id, prodId = item.ProductId, qty = item.Qty, price = item.PricePerItem, tax = item.TaxPercent }, trans);

                            await connection.ExecuteAsync("UPDATE inventory SET CurrentStock = CurrentStock + @qty WHERE Id = @prodId",
                                                     new { qty = item.Qty, prodId = item.ProductId }, trans);
                        }

                        await trans.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}
