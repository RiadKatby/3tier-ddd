using MCI.Mvc.Validation.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RefactorName.WebApp.Models
{
    public class Company
    {
        public Company()
        {
            Owners = new List<Owner>();
            SelectedOwners = new List<int>();
        }
        public int ID { get; set; }

        public string CRNO { get; set; }

        [Display(Name = "اسم المنشأة")]
        public string Name { get; set; }

        [Display(Name = "نشاطات الشركة")]
        public string Activities { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> CrExpirationDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsBig { get; set; }

        public string SecureCode { get; set; }

        public int TypeID { get; set; }

        public CompanyType CompanyType { get; set; }

        public CompanyStatus StatusID { get; set; }

        public virtual ICollection<Owner> Owners { get; set; }

        public List<int> SelectedOwners { get; set; }

    }

    public class Owner
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }

    public class CompanyType
    {
        public int ID { get; set; }

        [Display(Name = "نوع الشركة")]
        public string Name { get; set; }
    }

    public class CaptchaModel
    {
        [Captcha(ErrorMessage = "Please enter the correct number.")]
        [Required]
        public string Captcha { get; set; }
    }

    public class FileUploaderModel
    {
        public byte[] FileContent { get; set; }

        [Required]
        public string FilePath { get; set; }
    }

    public class SignatureModel
    {
        public byte[] Signature { get; set; }

        private string _signatureImageURL;
        public string SignatureImageURL
        {
            get
            {
                string result = this.Signature == null ? null : Convert.ToBase64String(this.Signature);
                return "data:image/png;base64," + result;
            }
            set
            {
                string theValue = value.ToString();
                byte[] data = string.IsNullOrEmpty(theValue) ? null : Convert.FromBase64String(theValue.Substring(theValue.IndexOf(",") + 1));
                this.Signature = data;
                this._signatureImageURL = theValue;
            }
        }
    }

    public class LocationModel
    {
        [Required(ErrorMessage = "Please choose your location")]
        public Nullable<Double> Lat { get; set; }

        [Required(ErrorMessage = "Please choose your location")]
        public Nullable<Double> Lng { get; set; }

        public string Title { get; set; }
    }

    public enum CompanyStatus
    {
        [Display(Name = "سارية")]
        Status1 = 1,
        [Display(Name = "موقوفة")]
        Status2,
        [Display(Name = "مرهونة")]
        Status3,
        [Display(Name = "مكسورة")]
        Status4
    }

    public enum CompanyTypes
    {
        [Display(Name = "مؤسسة")]
        Type1 = 1,
        [Display(Name = "مساهمة")]
        Type2,
        [Display(Name = "قابضة")]
        Type3,
        [Display(Name = "تعاونية")]
        Type4
    }

    public class TestEntity
    {
        [Required]
        public int ID { get; set; }

        [NatIDNumber(isSaudi = false, ErrorMessage = "الرجاء إدخال رقم هوية صحيح")]
        [Display(Name = "رقم الهوية")]
        public string IDNO { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        public bool bool1 { get; set; }

        public bool bool2 { get; set; }

        public bool bool3 { get; set; }

        public bool bool4 { get; set; }

        public bool bool5 { get; set; }

        public bool bool6 { get; set; }

        public bool bool7 { get; set; }

        public string SignatureImageURL { get; set; }

        public string FileName { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Birth { get; set; }

        public Nullable<Double> Lat { get; set; }
        public Nullable<Double> Lng { get; set; }

    }
}