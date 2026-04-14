namespace eg_mu.DTOs
{
    public class BookingCreateDTO
    {
        // --- بيانات الزائر الشخصية (التي تطلب في صفحة البيانات) ---
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public string Nationality { get; set; } // تم دمجها كحقل أساسي

        // --- بيانات التذكرة والحجز (التي تأتي من الاختيار والحساب) ---
        
        public string SelectedTimeSlot { get; set; } // الموعد المختار
        public int PriceId { get; set; } // هذا الرقم سيعرفنا كل شيء
        public int Quantity { get; set; }
        public DateOnly VisitDate { get; set; }
    }

    // الكلاسات المساعدة لعرض القائمة (لو وضعتها في نفس الملف)
    public class TicketMenuResponse
    {
        public string GroupName { get; set; }
        public List<TicketOptionDTO> Options { get; set; }
    }

    public class TicketOptionDTO
    {
        public int PriceId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }
}