using System;
using System.Collections.Generic;
using System.Text;
using cns.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cns.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //custom entity, override identity user with new column
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //custom entity, for simple todo app
        public DbSet<Todo> Todo { get; set; }
        //檔案表
        public DbSet<PDC_File> PDC_File { get; set; }
        //系統參數表
        public DbSet<PDC_Parameter> PDC_Parameter { get; set; }
        //Stackup欄位表
        public DbSet<PDC_StackupColumn> PDC_StackupColumn { get; set; }
        //Stackup欄位設定
        public DbSet<PDC_StackupDetail> PDC_StackupDetail { get; set; }
        //表單資料表
        public DbSet<PDC_Form> PDC_Form { get; set; }
        //表單紀錄表
        public DbSet<PDC_Form_StageLog> PDC_Form_StageLog { get; set; }
        //會員表
        public DbSet<PDC_Member> PDC_Member { get; set; }
        
        //部門對照表
        public DbSet<PDC_Department> PDC_Department { get; set; }
        //權限設定表
        public DbSet<PDC_Privilege> PDC_Privilege { get; set; }

        //表單查詢表
        public DbSet<vw_FormQuery> vw_FormQuery { get; set; }
        //權限查詢表
        public DbSet<vw_PrivilegeQuery> vw_PrivilegeQuery { get; set; }
    }
}
