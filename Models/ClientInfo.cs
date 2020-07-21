using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Kassa1.Models
{
    public class ClientInfo
    {
        [Display(Name = "Фамилия:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression("^([А-Я]|Ё){1}([а-я]|ё){1,29}$" , ErrorMessage = "Некорректная фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression("^([А-Я]|Ё){1}([а-я]|ё){1,29}$", ErrorMessage = "Некорректное имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression("^([А-Я]|Ё){1}([а-я]|ё|\\s){1,29}$", ErrorMessage = "Некорректное отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Дата рождения: ")]
        [Required(ErrorMessage = "Обязательное поле")]
        //[Range(typeof(DateTime), "01.02.1910", "03.04.2002")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Сумма займа:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [Range(1000, 10000, ErrorMessage = "от 1000 до 10 000")]
        public int LoanSum { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Шаблон:")]
        [RegularExpression(@".{1,}docx$", ErrorMessage = "Выберите шаблон")]
        public string TemplateName { get; set; }
    }
}