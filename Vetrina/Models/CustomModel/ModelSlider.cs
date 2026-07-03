using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vetrina.Models.CustomModel
{
    public class ModelSlider
    {
        internal AnyeLabelEntities db = new AnyeLabelEntities();
        public List<Slider> listaimmagini {
            get
            {
                return db.Slider.Where(x => x.eliminato != true && x.posizione == 1 && x.slider_num == 1).ToList();
            }
        }
        public Slider s1_p2 {
            get
            {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 2 && x.slider_num == 1).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        public Slider s1_p3 {
            get
            {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 3 && x.slider_num == 1).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        public Slider s1_p4 {
            get {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 4 && x.slider_num == 1).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        public Slider s2_p1 {
            get
            {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 1 && x.slider_num == 2).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        public Slider s2_p2 {
            get
            {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 2 && x.slider_num == 2).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        public Slider s2_p3 {
            get
            {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 3 && x.slider_num == 2).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        public Slider s2_p4 {
            get
            {
                Slider result = db.Slider.Where(x => x.eliminato != true && x.posizione == 4 && x.slider_num == 2).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }

        public Slider Message {
                get
            {
                Slider result = db.Slider.Where(x => x.posizione == 10).FirstOrDefault();
                return result == null ? new Slider() : result;
            }
        }
        
 
    }
}