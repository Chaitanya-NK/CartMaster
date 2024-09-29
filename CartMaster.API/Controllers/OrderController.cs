using CartMaster.Business.IServices;
using CartMaster.Business.Services;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Globalization;
using System.Net;

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IToken _token;
        public OrderController(IOrderService orderService, IToken token)
        {
            _orderService = orderService;
            _token = token;
        }

        [HttpPost("HandleOrder")]
        public IActionResult HandleOrder([FromQuery] string action, int orderId = 0, int userId = 0, decimal totalAmount = 0, string status = null, int orderItemId = 0, string returnStatus = null)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getbyorderid":
                        var order = _orderService.GetOrderDetailsByOrderId(orderId);
                        return Ok(order);

                    case "getbyuserid":
                        var userOrders = _orderService.ViewUserOrders(userId);
                        return Ok(userOrders);

                    case "add":
                        var createdOrder = _orderService.CreateOrder(userId, totalAmount);
                        return Ok(new { success = true, message = StaticProduct.AddProductSuccess });

                    case "updateorderstatus":
                        var updatedOrderStatus = _orderService.UpdateOrderStatus(orderId, status);
                        return Ok(new { success = true, message = StaticProduct.UpdateProductSuccess });

                    case "cancelorder":
                        var cancelledOrder = _orderService.CancelOrder(orderId);
                        return Ok(new { success = true, message = StaticOrder.CancelOrderSuccess });

                    case "requestreturn":
                        var requestReturnOrder = _orderService.RequestReturnByOrderItemId(orderItemId);
                        return Ok(new { success = true, message = StaticOrder.RequestReturnSuccess });

                    case "processreturn":
                        var processOrderReturn = _orderService.ProcessReturnByOrderItemId(orderItemId, returnStatus);
                        return Ok(new { success = true, message = StaticOrder.ProcessReturnSuccess });

                    default:
                        return BadRequest("Invalid action specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetOrderInvoice/{orderId}")]
        public IActionResult GetOrderInvoice(int orderId)
        {
            var order = _orderService.GetOrderDetailsByOrderId(orderId);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Create a new PDF document
                PdfDocument pdfDocument = new PdfDocument();
                PdfPage page = pdfDocument.AddPage();
                page.Orientation = PageOrientation.Landscape;
                page.Size = PageSize.A4;
                // Create an XGraphics object for drawing on the page
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Define fonts
                XFont titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
                XFont normalFont = new XFont("Verdana", 12, XFontStyle.Regular);
                XFont tableHeaderFont = new XFont("Verdana", 12, XFontStyle.Bold);

                // Define colors
                XBrush headerBrush = new XSolidBrush(XColor.FromArgb(0, 102, 204)); // Blue for headers
                XBrush borderBrush = XBrushes.Black; // Black border for table
                XPen tableBorderPen = new XPen(borderBrush, 1); // Table border thickness

                CultureInfo cultureInfo = new CultureInfo("hi-IN");

                // Draw the invoice title with color
                gfx.DrawString($"Invoice # {orderId}", titleFont, headerBrush,
                    new XRect(20, 20, page.Width, page.Height), XStringFormats.TopLeft);

                // Draw the order date
                gfx.DrawString($"Date: {order.OrderDate.ToShortDateString()}", normalFont, XBrushes.Black,
                    new XRect(20, 60, page.Width, page.Height), XStringFormats.TopLeft);

                // Draw the total amount with a bold font
                gfx.DrawString($"Total Amount: {order.TotalAmount.ToString("C", cultureInfo)}", new XFont("Verdana", 12, XFontStyle.Bold),
                    XBrushes.Black, new XRect(20, 90, page.Width, page.Height), XStringFormats.TopLeft);

                // Draw a separator line
                gfx.DrawLine(XPens.Gray, 20, 110, page.Width - 20, 110);

                // Define column widths
                double imageWidth = 150;
                double nameWidth = 150;
                double descriptionWidth = 150;
                double quantityWidth = 80;
                double priceWidth = 100;
                double totalWidth = imageWidth + nameWidth + descriptionWidth + quantityWidth + priceWidth;

                // Draw the table header with background color
                gfx.DrawRectangle(headerBrush, 20, 130, totalWidth, 20); // Background for header
                gfx.DrawString("Product Image", tableHeaderFont, XBrushes.White,
                    new XRect(25, 130, imageWidth, 20), XStringFormats.TopLeft);
                gfx.DrawString("Product Name", tableHeaderFont, XBrushes.White,
                    new XRect(25 + imageWidth, 130, nameWidth, 20), XStringFormats.TopLeft);
                gfx.DrawString("Product Description", tableHeaderFont, XBrushes.White,
                    new XRect(25 + imageWidth + nameWidth, 130, descriptionWidth, 20), XStringFormats.TopLeft);
                gfx.DrawString("Quantity", tableHeaderFont, XBrushes.White,
                    new XRect(25 + imageWidth + nameWidth + descriptionWidth, 130, quantityWidth, 20), XStringFormats.TopLeft);
                gfx.DrawString("Price", tableHeaderFont, XBrushes.White,
                    new XRect(25 + imageWidth + nameWidth + descriptionWidth + quantityWidth, 130, priceWidth, 20), XStringFormats.TopLeft);

                // Draw borders for the table header
                gfx.DrawRectangle(tableBorderPen, 20, 130, totalWidth, 20);

                // Draw the order items in a table-like format
                int yOffset = 160;
                foreach (var item in order.OrderItems)
                {
                    // Draw each row with alternating background color for better readability
                    XBrush rowBrush = (yOffset % 60 == 0) ? XBrushes.LightGray : XBrushes.White;
                    gfx.DrawRectangle(rowBrush, 20, yOffset, page.Width - 40, 20);

                    // Draw Product ID, Quantity, and Price
                    if (!string.IsNullOrEmpty(item.ImageURL))
                    {
                        try
                        {
                            WebRequest webRequest = WebRequest.Create(item.ImageURL);
                            using (var response = webRequest.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                XImage productImage = XImage.FromStream(() => stream);
                                gfx.DrawImage(productImage, 25, yOffset + 5, imageWidth - 10, 40);
                            }
                        }
                        catch(Exception ex)
                        {
                            gfx.DrawString("Image not available", normalFont, XBrushes.Red,
                                new XRect(25, yOffset + 5, imageWidth - 10, 40), XStringFormats.CenterLeft);
                        }
                    }
                    gfx.DrawString(item.ProductName.ToString(), normalFont, XBrushes.Black,
                        new XRect(25 + imageWidth, yOffset + 5, nameWidth, 40), XStringFormats.CenterLeft);
                    gfx.DrawString(item.ProductDescription.ToString(), normalFont, XBrushes.Black,
                        new XRect(25 + imageWidth + nameWidth, yOffset + 5, descriptionWidth, 40), XStringFormats.CenterLeft);
                    gfx.DrawString(item.Quantity.ToString(), normalFont, XBrushes.Black,
                        new XRect(25 + imageWidth + nameWidth + descriptionWidth, yOffset + 5, quantityWidth, 40), XStringFormats.CenterLeft);
                    gfx.DrawString(item.Price.ToString("C", cultureInfo), normalFont, XBrushes.Black,
                        new XRect(25 + imageWidth + nameWidth + descriptionWidth + quantityWidth, yOffset + 5, priceWidth, 40), XStringFormats.CenterLeft);

                    // Draw table row borders
                    gfx.DrawRectangle(tableBorderPen, 20, yOffset, totalWidth, 50);

                    yOffset += 60; // Move down for the next item
                }

                // Draw a separator line
                gfx.DrawLine(XPens.Gray, 20, 110, page.Width - 20, 110);

                // Draw a total section at the bottom of the table
                yOffset += 20;
                gfx.DrawString("Total", tableHeaderFont, headerBrush,
                    new XRect(25 + imageWidth + nameWidth + descriptionWidth, yOffset, quantityWidth, 20), XStringFormats.CenterLeft);
                gfx.DrawString(order.TotalAmount.ToString("C", cultureInfo), tableHeaderFont, headerBrush,
                    new XRect(25 + imageWidth + nameWidth + descriptionWidth + quantityWidth, yOffset, priceWidth, 20), XStringFormats.CenterLeft);

                // Draw a separator line
                gfx.DrawLine(XPens.Gray, 20, 110, page.Width - 20, 110);

                // Save the PDF into the memory stream
                pdfDocument.Save(memoryStream);

                // Return the generated PDF file
                return File(memoryStream.ToArray(), "application/pdf", $"Invoice_Order_{orderId}.pdf");
            }
        }
    }
}
