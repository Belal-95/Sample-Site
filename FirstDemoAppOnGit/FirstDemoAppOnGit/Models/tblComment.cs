//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstDemoAppOnGit.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblComment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string CommentBody { get; set; }
        public string NameOfUser { get; set; }
    
        public virtual tblPost tblPost { get; set; }
    }
}
