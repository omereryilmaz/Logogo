using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogoWebApi.Models;
using System.Web.Http;

namespace LogoWebApi.Controllers
{
    public class logogoController : ApiController
    {
        //
        // GET: /logogo/
        public List<MarkaLogo> Get()
        {
            return LogoKaynak.GetLogo();
        }

        public List<MarkaReklam> Getir(int id)
        {
            return LogoKaynak.GetReklam(id);
        }

        public  List<Dukkanlar> Getdukkan(int markaid,string sehir)
        {
            return LogoKaynak.GetDukkanlar(markaid,sehir);
        }

        


    }
}
