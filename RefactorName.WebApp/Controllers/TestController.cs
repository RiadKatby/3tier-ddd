using RefactorName.Web.Filters;
using RefactorName.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.Web.Controllers
{
    public class TestController : BaseController
    {
        #region Layout

        [HttpGet]
        public ActionResult TopMenu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SideMenu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Breadcrumb()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TopMessages()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TopMessages(string action)
        {
            switch (action)
            {
                case "error":
                    AddMCIMessage("Server Error Message", MCIMessageType.Danger);
                    break;
                case "sError":
                    AddMCIMessage("Server sticked Error Message", MCIMessageType.Danger, 0);
                    break;
                case "warning":
                    AddMCIMessage("Server warning Message", MCIMessageType.Warning);
                    break;
                case "info":
                    AddMCIMessage("Server info Message", MCIMessageType.Info);
                    break;
                case "success":
                    AddMCIMessage("Server success Message", MCIMessageType.Success);
                    break;
                default:
                    AddMCIMessage("Server Message", MCIMessageType.Success);
                    break;
            }
            return View();
        }

        #endregion

        #region Form items

        [HttpGet]
        public ActionResult DisplayItems()
        {
            var model = GetSampleCompany();
            return View(model);
        }

        [HttpGet]
        public ActionResult TextInputs()
        {
            var model = GetSampleCompany();
            return View(model);
        }

        [HttpGet]
        public ActionResult DropDownLists()
        {
            var model = GetSampleCompany();
            //fill ddls from database or enum
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();

            return View(model);
        }

        [Ajax]
        public ActionResult addl1ValueChanged(int? value)
        {
            return Content(string.Format("You chose value: {0}", value));
        }

        [Ajax]
        public ActionResult addl3ValueChanged(int? value, int? ddl1Value, int? ddl2Value, int? ddl3Value)
        {
            return Content(string.Format("ddl1 value: {1}{0}ddl2 value: {2}{0}ddl3 value: {3}{0}selected value: {4}", "<br/>", ddl1Value, ddl2Value, ddl3Value, value));
        }

        [HttpGet]
        public ActionResult CheckboxRadioButtonItems()
        {
            var model = GetSampleCompany();

            //selectlist of company types
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();

            //selectlist of company statuses
            ViewBag.CompanyStatuses = Util.EnumToSelectList<CompanyStatus>();

            return View(model);
        }

        [HttpGet]
        public ActionResult MultiSelectItems()
        {
            var model = GetSampleCompany();

            return View(model);
        }

        [HttpPost]
        [Ajax]
        public ActionResult MultiSelectItems(Company model, List<int> selectedOwners1)
        {
            string strSelected1 = selectedOwners1 == null ? "" : String.Join(",", selectedOwners1);
            string strSelected2 = String.Join(",", model.SelectedOwners);

            return Content(string.Format("Selected Values 1 = {0}<br/>Selected Values 2 = {1}", strSelected1, strSelected2));
        }

        [HttpGet]
        public ActionResult Captcha()
        {
            return View(new CaptchaModel());
        }

        [HttpPost]
        public ActionResult Captcha(CaptchaModel model)
        {
            if (!ModelState.IsValid)
            {
                //clear captcha field
                ModelState.SetModelValue("Captcha", new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentCulture));
                return View(model);
            }

            //proceed to next step
            AddMCIMessage("Yes!! This is the right captcha", MCIMessageType.Success);
            return View(model);
        }

        [HttpGet]
        public ActionResult FileUploader()
        {
            return View(new FileUploaderModel());
        }

        [HttpPost]
        public ActionResult FileUploader(FileUploaderModel model)
        {
            //get file content and set file content property
            if (!string.IsNullOrEmpty(model.FilePath))
            {
                model.FileContent = Util.GetArrayFromFile(Server.MapPath(model.FilePath), false);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Signature()
        {
            return View(new SignatureModel());
        }

        [HttpPost]
        public ActionResult Signature(SignatureModel model)
        {
            return View(model);
        }

        #endregion

        [HttpGet]
        public ActionResult Wizards()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Tabs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Modal()
        {
            return View();
        }

        [HttpGet]
        [Ajax]
        public ActionResult LoadAjaxModal()
        {
            System.Threading.Thread.Sleep(1000);
            return PartialView("_ModalPartialView");
        }

        #region Google Maps

        [HttpGet]
        public ActionResult GoogleMapsMarker()
        {
            var model = new LocationModel();
            //var model = new LocationModel()
            //{
            //    Lat = 24.656393,
            //    Lng = 46.715118
            //};

            return View(model);
        }

        [HttpPost]
        public ActionResult GoogleMapsMarker(LocationModel model)
        {
            return View(model);
        }

        [HttpGet]
        public ActionResult GoogleMapsMarkers()
        {
            List<GoogleMarker> model = FillSampleMarkers();
            return View(model);
        }

        private List<GoogleMarker> FillSampleMarkers()
        {
            var result = new List<GoogleMarker>();

            result.Add(new GoogleMarker() { ID = 0, Title = "marker1", Lat = 24.656393, Lng = 46.715118 });
            result.Add(new GoogleMarker() { ID = 1, Title = "marker2", Lat = 24.653268, Lng = 46.712264 });
            result.Add(new GoogleMarker() { ID = 2, Title = "marker3", Lat = 24.651220, Lng = 46.727349 });
            result.Add(new GoogleMarker() { ID = 3, Title = "marker4", Lat = 24.665427, Lng = 46.714152 });

            return result;
        }

        #endregion

        [HttpGet]
        public ActionResult Links()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Buttons()
        {
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();
            return View();
        }

        [HttpPost]
        public ActionResult Buttons(string command)
        {
            return View();
        }

        [Ajax]
        public ActionResult ReturnAjaxErrorMessage()
        {
            return JsonErrorMessage("I used return JsonErrorMessage(<the message>) to return this message to Failure event. it is in result.responseJSON.message");
        }

        [HttpPost]
        [Ajax]
        public ActionResult LoadAjaxContent(int example, int? ddl1, string txt1)
        {
            Thread.Sleep(2000);
            if (example == 2)
                return JsonErrorMessage("Some thing is wrong!");

            else if (example == 1)
                return Content("This is our response");

            else if (example == 3)
            {
                string result = "This is our response: ";
                if (ddl1.HasValue)
                    result += string.Format("ddl1:{0}. ", ddl1);
                if (!string.IsNullOrEmpty(txt1))
                    result += string.Format("txt1:{0}. ", txt1);
                return Content(result);
            }
            else
                return Content("This is our response");

        }

        #region Plugins

        #region Star rating

        public ActionResult StarRating()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StarRating(int? rating1, int? rating2, int? rating3, int? rating4, int? rating5, int? rating6)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeRating(int value)
        {
            System.Threading.Thread.Sleep(2000);
            if (value == 5)
                throw new Exception();
            return Json(new { });
        }
        #endregion

        #region Date Picker

        [HttpGet]
        public ActionResult DatePicker()
        {
            return View();
        }

        #endregion

        #region Multi Select

        [HttpGet]
        public ActionResult MultiSelect()
        {
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();
            return View();
        }

        [HttpPost]
        public ActionResult MultiSelect(int? multiSelect1, int? multiSelect2, List<int> multiSelect3)
        {
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();
            return View();
        }

        #endregion

        #region Select2

        [HttpGet]
        public ActionResult Select2()
        {
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();
            return View();
        }

        [HttpPost]
        public ActionResult Select2(int? select21, List<int> select22)
        {
            ViewBag.CompanyTypes = Util.EnumToSelectList<CompanyTypes>();
            return View();
        }

        #endregion

        #region Pretty Photo
        [HttpGet]
        public ActionResult PrettyPhoto()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetSamplePhoto()
        {
            var theFile = Util.GetArrayFromFile(Server.MapPath("~/images/profilePic.jpg"), false);
            return File(theFile, "image/png");
        }

        #endregion

        #region Type Ahead
        [HttpGet]
        public ActionResult TypeAhead()
        {
            return View();
        }

        [Ajax]
        [HttpGet]
        public ActionResult GetCompanies(string name)
        {
            List<Company> companies = GetSampleCompanies(name);
            return Json(companies, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Input filter
        [HttpGet]
        public ActionResult FilterInput()
        {
            return View();
        }

        #endregion

        #region Input Mask

        [HttpGet]
        public ActionResult InputMask()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InputMask(int? inputMask8, string inputMask4)
        {
            return View();
        }

        #endregion
        #endregion

        #region MCI Charts

        public ActionResult PieChart()
        {
            MciChart piechart = FillSamplePieChartData();
            return View(piechart);
        }

        private MciChart FillSamplePieChartData()
        {
            MciChart piechart = new MciChart() { Title = "Pie Chart Title", SubTitle = "Pie Chart Sub Title" };
            piechart.AddMciChartCategory("الرياض").AddMciChartCategoryValues("الزيارات", 1800).AddMciChartCategoryValues("المخالفات", Value: 1500).AddMciChartCategoryValues("التجمعات", 1700).AddMciChartCategoryValues("الظروف", 1600);
            piechart.AddMciChartCategory("الدمام").AddMciChartCategoryValues("الزيارات", 1800).AddMciChartCategoryValues("المخالفات", Value: 1500).AddMciChartCategoryValues("التجمعات", 1700).AddMciChartCategoryValues("الظروف", 1600);
            piechart.AddMciChartCategory("جدة").AddMciChartCategoryValues("الزيارات", 1800).AddMciChartCategoryValues("المخالفات", Value: 1500).AddMciChartCategoryValues("التجمعات", 1700).AddMciChartCategoryValues("الظروف", 1600);
            piechart.AddMciChartCategory("نجران").AddMciChartCategoryValues("الزيارات", 1800).AddMciChartCategoryValues("المخالفات", Value: 1500).AddMciChartCategoryValues("التجمعات", 1700).AddMciChartCategoryValues("الظروف", 1600);
            return piechart;
        }

        public ActionResult BarChart()
        {
            MciChart barchart = FillSampleBarChartData();

            return View(barchart);

        }

        private MciChart FillSampleBarChartData()
        {
            MciChart barchart = new MciChart() { Title = "Bar Chart Title", SubTitle = "Bar Chart Sub Title", Data = new List<MciChartCategory>() };

            barchart.Data = barchart.AddMciChartCategory("محرم").AddMciChartCategoryValues("الزيارات", 1).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("صفر").AddMciChartCategoryValues("الزيارات", 4).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("ربيع أول").AddMciChartCategoryValues("الزيارات", 2).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("ربيع ثان").AddMciChartCategoryValues("الزيارات", 3).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("جماد اول").AddMciChartCategoryValues("الزيارات", 1).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("جماد ثان").AddMciChartCategoryValues("الزيارات", 2).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("رجب").AddMciChartCategoryValues("الزيارات", 3).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("شعبان").AddMciChartCategoryValues("الزيارات", 4).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("رمضان").AddMciChartCategoryValues("الزيارات", 3).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("شوال").AddMciChartCategoryValues("الزيارات", 2).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("ذي القعدة").AddMciChartCategoryValues("الزيارات", 1).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4)
                                    .AddMciChartCategory("ذي الحجة").AddMciChartCategoryValues("الزيارات", 3).AddMciChartCategoryValues("المخالفات", 2.5f).AddMciChartCategoryValues("التجمعات", 3).AddMciChartCategoryValues("الظروف", 4);

            return barchart;
        }

        public ActionResult MapChart()
        {
            MciChart mapChart = FillSampleMapChartData();
            return View(mapChart);
        }

        private MciChart FillSampleMapChartData()
        {
            var mapchart = new MciChart() { Title = "Map Chart Title", SubTitle = "Map Chart Sub Title", Data = new List<MciChartCategory>() };

            var data = new List<MciChartCategory>(){ 
                new MciChartCategory()
                {
                    Category = "sa-ri",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 1}
                    ,new MciChartValues(){ Name="المخالفات", Value = 2.5f}
                    ,new MciChartValues(){ Name="التجمعات", Value = 3  }
                    ,new MciChartValues(){ Name="الظروف", Value = 4}}
                },
                new MciChartCategory()
                {
                    Category = "sa-mk",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 2}
                    ,new MciChartValues(){ Name="المخالفات", Value = 3}
                    ,new MciChartValues(){ Name="التجمعات", Value = 4  }
                    ,new MciChartValues(){ Name="الظروف", Value = 1}}
                },
                new MciChartCategory()
                {
                    Category = "sa-md",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 3}
                    ,new MciChartValues(){ Name="المخالفات", Value = 4}
                    ,new MciChartValues(){ Name="التجمعات", Value = 1 }
                    ,new MciChartValues(){ Name="الظروف", Value = 2}}
                },
                new MciChartCategory()
                {
                    Category = "sa-qs",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 4}
                    ,new MciChartValues(){ Name="المخالفات", Value = 1}
                    ,new MciChartValues(){ Name="التجمعات", Value = 2 }
                    ,new MciChartValues(){ Name="الظروف", Value = 3}}
                },
                new MciChartCategory()
                {
                    Category = "sa-sh",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 1}
                    ,new MciChartValues(){ Name="المخالفات", Value = 2}
                    ,new MciChartValues(){ Name="التجمعات", Value = 3}
                    ,new MciChartValues(){ Name="الظروف", Value = 4}}
                },
                new MciChartCategory()
                {
                    Category = "sa-as",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 2}
                    ,new MciChartValues(){ Name="المخالفات", Value = 3}
                    ,new MciChartValues(){ Name="التجمعات", Value = 4}
                    ,new MciChartValues(){ Name="الظروف", Value = 1}}
                },
                new MciChartCategory()
                {
                    Category = "sa-ha",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 3}
                    ,new MciChartValues(){ Name="المخالفات", Value = 4}
                    ,new MciChartValues(){ Name="التجمعات", Value = 1 }
                    ,new MciChartValues(){ Name="الظروف", Value = 2}}
                },
                new MciChartCategory()
                {
                    Category = "sa-tb",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 4}
                    ,new MciChartValues(){ Name="المخالفات", Value = 1}
                    ,new MciChartValues(){ Name="التجمعات", Value = 2 }
                    ,new MciChartValues(){ Name="الظروف", Value = 3}}
                },
                new MciChartCategory()
                {
                    Category = "sa-ba",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 1}
                    ,new MciChartValues(){ Name="المخالفات", Value = 2}
                    ,new MciChartValues(){ Name="التجمعات", Value = 3 }
                    ,new MciChartValues(){ Name="الظروف", Value = 4}}
                },
                new MciChartCategory()
                {
                    Category = "sa-hs",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 2}
                    ,new MciChartValues(){ Name="المخالفات", Value = 3}
                    ,new MciChartValues(){ Name="التجمعات", Value = 4 }
                    ,new MciChartValues(){ Name="الظروف", Value = 1}}
                },
                new MciChartCategory()
                {
                    Category = "sa-jf",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 3}
                    ,new MciChartValues(){ Name="المخالفات", Value = 4}
                    ,new MciChartValues(){ Name="التجمعات", Value = 1}
                    ,new MciChartValues(){ Name="الظروف", Value = 2}}
                },
                new MciChartCategory()
                {
                    Category = "sa-jz",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 4}
                    ,new MciChartValues(){ Name="المخالفات", Value = 1}
                    ,new MciChartValues(){ Name="التجمعات", Value = 2 }
                    ,new MciChartValues(){ Name="الظروف", Value = 3}}
                },
                new MciChartCategory()
                {
                    Category = "sa-nj",
                    Values = new List<MciChartValues>(){ 
                    new MciChartValues(){ Name="الزيارات", Value = 4}
                    ,new MciChartValues(){ Name="المخالفات", Value = 1}
                    ,new MciChartValues(){ Name="التجمعات", Value = 2}
                    ,new MciChartValues(){ Name="الظروف", Value = 3}}
                }
            };

            mapchart.Data.AddRange(data);

            return mapchart;

        }


        #endregion

        public ActionResult Index()
        {
            AddMCIMessage("فقط معلومات , ملاحظة فقط معلومات , ملاحظة", MCIMessageType.Info, 0);
            //MCIAlert.Alert(this, "حدث خطأ, الرجاء المحاولة ثانية", AlertType.Danger, 10);
            //MCIAlert.Alert(this, "تم ارسال البيانات بنجاح", AlertType.Success, 10);
            //MCIAlert.Alert(this, "الرجاء الانتباه, يجب الموافقة على شروط التسجيل أولاً", AlertType.Warning, 0);
            FillSampleData();
            return View(new TestEntity());
        }

        [HttpPost]
        public ActionResult Index(Models.TestEntity model, string action, string testText, DateTime? datePicker1)
        {
            FillSampleData();
            AddMCIMessage("submitted: action:" + action);
            return View(model);
        }

        [OutputCache(VaryByParam = "*", NoStore = true, Duration = 0)]
        public ActionResult GetData(int id)
        {
            System.Threading.Thread.Sleep(3000);
            return PartialView("ViewData");
        }

        public ActionResult testAjax()
        {
            System.Threading.Thread.Sleep(3000);
            return Content("return value");
        }

        [HttpPost]
        public ActionResult addl1Test(int? Value)
        {
            if (!Value.HasValue)
                return JsonErrorMessage("value can't be null");

            if (Value == 1)
                return JsonErrorMessage("your value can't be 1");
            return Content("addl1Test success: Value = " + Value.ToString());
        }

        public void FillSampleData()
        {
            ViewData["testGridItems"] = new List<TestEntity>(){
                new TestEntity(){ID=1,Name="محمد  حمدان",Age=19}
                ,new TestEntity(){ID=2,Name="مراد السبيعي",Age=19}
                 ,new TestEntity(){ID=3,Name="عبد الله الحمود",Age=19}
                 ,new TestEntity(){ID=4,Name="فهد الغامدي",Age=19}
                 ,new TestEntity(){ID=5,Name="تركي التركي",Age=19}
                 ,new TestEntity(){ID=6,Name="سعيد السعيدان",Age=19}
                 ,new TestEntity(){ID=7,Name="رامي السعدان",Age=19}
                 ,new TestEntity(){ID=8,Name="محمد المويه",Age=39}
                 ,new TestEntity(){ID=9,Name="سعود الاحمد",Age=23}

            };

            Session["testGridItems"] = ViewData["testGridItems"];

            ViewData["cbSample"] = new SelectList(new List<SelectListItem>() {
                new SelectListItem{Value="True", Text = "خيار 1"},
                new SelectListItem{Value="True", Text = "خيار 2"},
                new SelectListItem{Value="True", Text = "خيار 3"}
            }, "Value", "Text");

            ViewData["rbSample"] = new SelectList(new List<SelectListItem>() {
                new SelectListItem{Value="Yes", Text = "نعم"},
                new SelectListItem{Value="No", Text = "لا"},
                new SelectListItem{Value="Sometimes", Text = "أحياناً"}
            }, "Value", "Text", "Yes");


        }

        private Company GetSampleCompany(int ID = 1)
        {
            return new Company()
            {
                Activities = "Activity1, Activity2, Activity3, Activity4, Activity5, Activity6, Activity7, Activity8, Activity9, Activity10, Activity11, Activity12, Activity13, Activity14",
                CreationDate = DateTime.Now.AddYears(-1),
                CrExpirationDate = DateTime.Now.AddYears(1),
                CRNO = (ID.ToString() + "xxxxxxxxxx").Substring(0, 10),
                ID = ID,
                IsActive = true,
                Name = "شركة ثقة لخدمات الأعمال " + ID.ToString(),
                TypeID = 4,
                StatusID = CompanyStatus.Status1,
                CompanyType = new CompanyType() { ID = 4, Name = "تعاونية" },
                Owners = new List<Owner>(){
                    new Owner(){ID=1,Name="Owner 1"},
                    new Owner(){ID=2,Name="Owner 2"},
                    new Owner(){ID=3,Name="Owner 3"},
                    new Owner(){ID=4,Name="Owner 4"},
                    new Owner(){ID=5,Name="Owner 5"},
                    new Owner(){ID=6,Name="Owner 6"}
                },
                SelectedOwners = new List<int>() { 1, 3, 5 }
            };
        }

        private List<Company> GetSampleCompanies(string name)
        {
            List<Company> result = new List<Company>();
            Company company;
            for (int i = 1; i <= 5; i++)
            {
                company = GetSampleCompany(i);
                company.Name = name + company.Name;
                result.Add(company);
            }
            return result;
        }

        #region Edit Sample

        [Ajax]
        public ActionResult EditOpen(int ID = 0)
        {
            string errMSG = "";
            try
            {
                if (ID == 1)
                {
                    errMSG = "not allowed";
                    return JsonErrorMessage("غير مسموح");
                    //throw new Exception();
                }
                System.Threading.Thread.Sleep(2000);
                var itemToEdit = (Session["testGridItems"] as List<TestEntity>).FirstOrDefault(d => d.ID == ID);
                return PartialView("_EditForm", itemToEdit);
            }
            catch
            {
                if (string.IsNullOrEmpty(errMSG))
                {
                    errMSG = "DB Error";
                }
                throw new Exception(errMSG);
            }
        }
        [HttpGet]
        public ActionResult Edit()
        {
            FillSampleData();
            return PartialView("_WebgridSample", ViewData["testGridItems"]);
        }
        [HttpPost]
        public ActionResult Edit(TestEntity model)
        {
            FillSampleData();
            return PartialView("_WebgridSample", ViewData["testGridItems"]);

        }

        [HttpPost]
        public ActionResult DeleteAction(int id)
        {
            try
            {
                //delete
                var data = (List<TestEntity>)Session["testGridItems"];
                data.Remove(data.Where(d => d.ID == id).FirstOrDefault());
                return PartialView("_WebgridSample", data);
            }
            catch
            {
                return JsonErrorMessage("عفواً. حدث خطأ أثناء حذف العنصر");
            }
        }

        [HttpGet] //for paging and sorting
        [Ajax]
        public ActionResult DeleteAction()
        {
            System.Threading.Thread.Sleep(1000);
            return PartialView("_WebgridSample", Session["testGridItems"]);
        }
        #endregion

    }
}
