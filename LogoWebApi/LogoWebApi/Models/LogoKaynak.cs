using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogoWebApi.Models
{
    public class LogoKaynak 
    {
        public static List<MarkaLogo> GetLogo()
        {
            omermobil_dbEntities1 dataContext = new omermobil_dbEntities1();
            var sorgu = from logo in  dataContext.MarkaLogoes
                        where logo.seviye==1
                        orderby logo.oncelik descending
                        select logo;
            return sorgu.ToList();
        }

        public static List<MarkaReklam> GetReklam(int markaid)
        {
            omermobil_dbEntities1 dataContext = new omermobil_dbEntities1();
            var query = from c in dataContext.MarkaReklams

                        where c.markaid == markaid
                        select c;
            return query.ToList();
        }

        
            public static List<Dukkanlar> GetDukkanlar(int markaid,string sehir)
            {
                omermobil_dbEntities1 dataContext = new omermobil_dbEntities1();
                var sorgu = from d in dataContext.Dukkanlars
                            where d.markaid==markaid && d.sehir.Contains(sehir) 
                            select d;
                return sorgu.ToList();
            }
        
    }
}
