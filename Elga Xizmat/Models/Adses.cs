//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elga_Xizmat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Adses
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> Rubric_Id { get; set; }
        public Nullable<int> TypeProduct_Id { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Short_text { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> Area_Id { get; set; }
        public string Picture { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> State_ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> CountView { get; set; }
        public Nullable<int> TypeAdses { get; set; }
    }
}
