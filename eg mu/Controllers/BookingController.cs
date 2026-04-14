using eg_mu.DTOs;
using eg_mu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class BookingController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public BookingController(GrandEgyptianContext context)
        {
            _context = context;
        }




        /// <summary>
        /// 3. عرض مواعيد الزيارة المتاحة
        /// </summary>
        [HttpGet("available-slots")]
        public IActionResult GetAvailableSlots()
        {
            var slots = new List<string>
    {
        "09:00 AM - 11:00 AM",
        "11:00 AM - 01:00 PM",
        "01:00 PM - 03:00 PM",
        "03:00 PM - 05:00 PM",
        "05:00 PM - 07:00 PM",
        "07:00 PM - 09:00 PM"
    };
            return Ok(slots);
        }




        /// <summary>
        /// 1. عرض قائمة الأسعار (متاحة للجميع)
        /// </summary>
        [HttpGet("ticket-menu")]
        public IActionResult GetTicketMenu()
        {
            var menu = new List<TicketMenuResponse>
            {
                new TicketMenuResponse {
                    GroupName = "المصريين",
                    Options = new List<TicketOptionDTO> {
                        new TicketOptionDTO { PriceId = 1, CategoryName = "مصري - بالغ (200.00 جنيه مصري)", Price = 200.00m },
                        new TicketOptionDTO { PriceId = 2, CategoryName = "مصري - طالب (100.00 جنيه مصري)", Price = 100.00m },
                          new TicketOptionDTO { PriceId = 3, CategoryName = "مصري - طفل (100.00 جنيه مصري)", Price = 100.00m },
                        new TicketOptionDTO { PriceId = 4, CategoryName = "كبار السن مصريين فوق 60 عاماً (100.00 جنيه مصري)", Price = 100.00m }

                    }
                },
                new TicketMenuResponse {
                    GroupName = "العرب",
                    Options = new List<TicketOptionDTO> {
                        new TicketOptionDTO { PriceId = 5, CategoryName = "عربي - بالغ (1450.00 جنيه مصري)", Price = 1450.00m },
                        new TicketOptionDTO { PriceId = 6, CategoryName = "عربي - طالب (730.00 جنيه مصري)", Price = 730.00m },
                          new TicketOptionDTO { PriceId = 7, CategoryName = "عربي - طفل (730.00 جنيه مصري)", Price = 730.00m }
                    }
                },
                new TicketMenuResponse {
                    GroupName = "الأجانب",
                    Options = new List<TicketOptionDTO> {
                        new TicketOptionDTO { PriceId = 8, CategoryName = "أجنبي - بالغ (1450.00 جنيه مصري)", Price = 1450.00m },
                        new TicketOptionDTO { PriceId = 9, CategoryName = "أجنبي - طالب (730.00 جنيه مصري)", Price = 730.00m },
                         new TicketOptionDTO { PriceId = 10, CategoryName = "أجنبي - طفل (730.00 جنيه مصري)", Price = 730.00m }
                    }
                }
            };
            return Ok(menu);
        }

        /// <summary>
        /// 2. تنفيذ الحجز مع التحقق المباشر من بيانات الزائر
        /// </summary>
        [HttpPost("submit-booking")]
        public async Task<IActionResult> SubmitBooking([FromBody] BookingCreateDTO dto)
        {
            // أ. التحقق من وجود الزائر في قاعدة البيانات (الاسم، الإيميل، الباسورد)
            // هذا الجزء يحل محل [Authorize] ويضمن أن البيانات صحيحة
            var visitor = await _context.Visitors.FirstOrDefaultAsync(v =>
                v.Email == dto.Email &&
                v.PasswordHash == dto.Password &&
                v.FirstName == dto.FirstName &&
                v.LastName == dto.LastName);

            if (visitor == null)
            {
                return Unauthorized(new
                {
                    Message = "فشل التحقق: بيانات الزائر أو كلمة المرور غير صحيحة. يرجى التأكد من بياناتك أو عمل حساب جديد."
                });
            }

            // ب. خريطة الأسعار الثابتة (من الكود الخاص بك)
            var priceMapping = new Dictionary<int, decimal>
            {
                { 1, 200.00m }, { 2, 100.00m }, { 3, 100.00m },
                { 4, 100.00m }, { 5, 1450.00m }, { 6, 730.00m }, { 7, 730.00m } ,
                { 8, 1450.00m } , { 9, 730.00m } , { 10, 730.00m }
            };

            // ج. التحقق من أن فئة التذكرة موجودة
            if (!priceMapping.ContainsKey(dto.PriceId))
            {
                return BadRequest("فئة تذكرة غير معرفة، يرجى اختيار تذكرة صحيحة.");
            }

            // د. حساب المبلغ الإجمالي
            decimal pricePerUnit = priceMapping[dto.PriceId];
            int quantity = dto.Quantity > 0 ? dto.Quantity : 1;
            decimal totalAmount = pricePerUnit * quantity;

            // هـ. إنشاء سجل الحجز وربطه بالزائر الذي تم العثور عليه
            var newBooking = new Booking
            {
                VisitorId = visitor.VisitorId, // الربط بالـ ID الصحيح من الداتابيز
                PriceId = dto.PriceId,
                Nationality = dto.Nationality,
                Quantity = quantity,
                VisitDate = dto.VisitDate,
                TotalAmount = totalAmount,
                BookingDate = DateTime.Now,
                
            };

            // و. الحفظ النهائي في قاعدة البيانات
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            // ز. الرد برسالة نجاح منظمة
            return Ok(new
            {
                Message = "تم الحجز بنجاح ✅",
                BookingReference = newBooking.BookingId,
                VisitorName = $"{visitor.FirstName} {visitor.LastName}",
                TotalAmount = $"{totalAmount} جنيه مصري",
                Details = $"تم حجز {quantity} تذكرة لليوم {dto.VisitDate.ToShortDateString()}"
            });
        }
    }
}