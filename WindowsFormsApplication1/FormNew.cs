﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormNew : Form
    {
        public FormNew()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string pc = "";
            ComboboxItem si = (ComboboxItem)cb_pc.SelectedItem;
            try
            {
                pc = si.Text;
                if (si.Value.ToString() == "")
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                pc = cb_pc.Text;
            }
            
            //sheet pc duplicate            
            string sqlStr = "SELECT count(*) FROM sheets WHERE pc = '" + pc + "'";
            int scount = Int16.Parse(DB.excuteScalar(sqlStr));
            Form1 f = (Form1)this.Owner;
            if (scount > 0)
            {
                //open this
                sqlStr = "UPDATE config SET pc = '" + pc + "'";
                DB.excuteSql(sqlStr);
                //start recruit
            }
            else
            {
                //insert sheet
                sqlStr = "INSERT INTO sheets(pc) VALUES('" + pc + "')";
                DB.excuteSql(sqlStr);
                //open this
                sqlStr = "UPDATE config SET pc = '" + pc + "'";
                DB.excuteSql(sqlStr);
                //start recruit
            }
            f.pc = pc;
            f.startRecruit();
            Close();
        }

        private void FormNew_Load(object sender, EventArgs e)
        {
            string sqlStr = "SELECT distinct(pc) FROM sheets";  //zy

            OleDbDataReader dr = DB.dataReader(sqlStr);
            cb_pc.Items.Clear();
            cb_pc.Items.Add(new ComboboxItem("——请选择——", ""));
            while (dr.Read())
            {
                cb_pc.Items.Add(new ComboboxItem(dr["pc"].ToString(), dr["pc"].ToString()));
            }
            dr.Close();
            cb_pc.SelectedIndex = 0;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCreate fc = new FormCreate();
            fc.Owner = this;
            fc.ShowDialog();
        }
    }
}

