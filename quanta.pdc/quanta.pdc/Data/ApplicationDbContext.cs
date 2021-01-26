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
    }
}
