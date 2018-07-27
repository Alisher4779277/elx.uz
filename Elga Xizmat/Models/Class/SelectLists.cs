using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elga_Xizmat.Models;
using System.Web.Mvc;

namespace Elga_Xizmat
{
    public class SelectLists
    {
        private static ELX_DBEntities db = new ELX_DBEntities();

        public static List<SelectListItem> Area(int lang)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectList select = new SelectList(db.Areas, "Id", "AreaNameUzb");
            list.AddRange(select);
            return list;
        }

        public static List<SelectListItem> Region(int lang)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectList select = new SelectList(db.Regions, "Id", "Name");
            list.AddRange(select);
            return list;
        }

        public static List<SelectListItem> Rubric(int lang)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectList select = new SelectList(db.Rubric, "Id", "Name_Uz");
            list.AddRange(select);
            return list;
        }

        public static List<SelectListItem> Type_Product(int lang)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectList select = new SelectList(db.Type_Product, "Id", "Name_Uz");
            list.AddRange(select);
            return list;
        }
    }
}