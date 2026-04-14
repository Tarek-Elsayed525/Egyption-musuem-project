using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eg_mu.Models;

public partial class Visitor
{
    // 1. الحقول الأساسية (تُنشأ كأعمدة في قاعدة البيانات)
    [Key]
    public int VisitorId { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$",
        ErrorMessage = "عذراً، يجب استخدام حساب Gmail أو Yahoo فقط")]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.Now;


    // 2. الحقول المضافة يدوياً (غير موجودة في جدول قاعدة البيانات)
    // نستخدم [NotMapped] لمنع EF من البحث عن هذه الأعمدة في الجدول

    [NotMapped]
    public string? ConfirmPassword { get; set; } // تم تغييره لـ string بدلاً من object للتعامل معه في الـ Register

    [NotMapped]
    public string? Nationality { get; set; } // تم تغييره لـ public ليتمكن الـ Controller من الوصول إليه


    // 3. العلاقات (الربط مع الجداول الأخرى)
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}