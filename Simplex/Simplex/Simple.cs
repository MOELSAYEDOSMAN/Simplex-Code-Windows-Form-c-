using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Simplex
{
    class Simple

    {

        //type= max
        string returnMaxString(Dictionary<string, double> ls)
        {
            double m = 0; string a = "";
            foreach (var s in ls)
            {
                if (s.Value > m && s.Value > 0)
                {
                    m = s.Value;
                    a = s.Key;
                }
            }

            return a;
        }

        string returnminstring(Dictionary<string, double> ls)
        {
            var lss = ls.ToList();
            double m = double.MaxValue; string a = "";
            foreach (var s in ls)
            {
                if ((s.Value > 0 || s.Value > m) || (m <= 0 && s.Value > 0))
                {
                    m = s.Value;
                    a = s.Key;
                }
            }

            return a;
        }

        int minRHP(List<double> ls)
        {
            int m = 0; double a = ls[0];
            for (int i = 0; i < ls.Count; i++)
            {
                if ((a <= 0 && a < ls[i] && ls[i] > 0) || (ls[i] > 0 && ls[i] < a))
                {
                    a = ls[i];
                    m = i;
                }
            }
            return m;
        }


        //type= min
        string returnminTominString(Dictionary<string, double> ls)
        {
            double m = 0; string a = "";
            foreach (var s in ls)
            {
                if (s.Value < m && s.Value < 0)
                {
                    m = s.Value;
                    a = s.Key;
                }
            }

            return a;
        }

        string returnminToMinstring(Dictionary<string, double> ls)
        {
            var lss = ls.ToList();
            double m = double.MaxValue; string a = "";
            foreach (var s in ls)
            {
                if ((s.Value < 0 || s.Value < m) || (m >= 0 && s.Value < 0))
                {
                    m = s.Value;
                    a = s.Key;
                }
            }

            return a;
        }

        public double sim(DataGridView g1, DataGridView g2, string type, ref DataTable DtTable)
        {

            double res = 0;
            if (type == "Max")
            {
                Dictionary<string, List<double>> Values = new Dictionary<string, List<double>>();
                Dictionary<string, double> Cj = new Dictionary<string, double>();
                int c1 = g1.ColumnCount - 1, c2 = g2.ColumnCount - 1, r1 = g1.RowCount - 2, r2 = g2.RowCount - 2;
                List<string> zj = new List<string>();
                Dictionary<string, double> zj1 = new Dictionary<string, double>();
                Dictionary<string, double> cj_zj = new Dictionary<string, double>();
                List<double> RHP = new List<double>();
                List<string> Names = new List<string>();

                //fill Cj from g2
                int x = 1;
                for (int j = 0; j <= c2; j++)
                {
                    int num = int.Parse(g2[j, 0].Value.ToString());
                    Names.Add($"x{x}");
                    Values.Add($"x{x}", new List<double>());
                    Cj.Add($"x{x++}", num);

                }

                //add R.h.p and Count Varible and Fill Cj from g1 and zj
                x = 1;
                int a = 1; for (int i = 0; i <= r1; i++)
                {
                    string s, A;
                    if (g1[c1 - 1, i].Value.ToString() == ">=")
                    {
                        s = $"S{x}"; A = $"A{a}";
                        //R.H.P ADD
                        RHP.Add(double.Parse(g1[c1, i].Value.ToString()));
                        //Cj Add
                        Cj.Add(s, 0);//s
                        Cj.Add(A, -.5);//a
                                       //Add To Names
                        Names.Add(s);
                        Names.Add(A);
                        //names to value
                        Values.Add(s, new List<double>());
                        Values.Add(A, new List<double>());
                        //fill zj
                        zj.Add(A);
                        x++;
                        a++;

                    }
                    else if (g1[c1 - 1, i].Value.ToString() == "<=")
                    {
                        s = $"S{x}";
                        //R.H.P ADD
                        RHP.Add(double.Parse(g1[c1, i].Value.ToString()));
                        //Cj Add
                        Cj.Add(s, 0);//s
                                     // Cj.Add(A, -.5);//a
                                     //Add To Names
                        Names.Add(s);
                        // Names.Add(A);
                        //names to value
                        Values.Add(s, new List<double>());
                        //fill Zj
                        zj.Add(s);
                        x++;
                    }
                    else
                    {
                        A = $"A{a}";
                        //R.H.P ADD
                        RHP.Add(double.Parse(g1[c1, i].Value.ToString()));
                        //Cj Add
                        //Cj.Add(s, 0);//s
                        Cj.Add(A, -.5);//a
                                       //Add To Names
                                       //Names.Add(s);
                        Names.Add(A);
                        //fill Value
                        Values.Add(A, new List<double>());
                        //fill ZJ
                        zj.Add(A);
                        a++;
                    }
                }

                //Fill Values form f2
                x = 1;
                for (int i = 0; i <= c1 - 2; i++)
                {
                    string v = $"x{x++}";
                    for (int j = 0; j <= r1; j++)
                    {
                        Values[v].Add(double.Parse(g1[i, j].Value.ToString()));
                    }
                }
                //Fill S
                List<string> ls_S = Names.Where(S => S.Contains('S') || S.Contains('A')).ToList();
                List<string> ls_S1 = Names.Where(S => S.Contains('S')).ToList();
                x = 1;
                foreach (string s in ls_S1)
                {
                    int val = 1;
                    var serch = ls_S.SingleOrDefault(ss => ss.Contains($"A{x}"));
                    if (serch != "" && serch != null)
                    {
                        val = -1;

                    }
                    for (int i = 0; i <= r1; i++)
                    {
                        if (x - 1 == i)
                        {
                            Values[s].Add(val);
                        }
                        else
                        {
                            Values[s].Add(0);
                        }
                    }
                    x++;
                }
                //Fill A
                List<string> ls_A1 = Names.Where(S => S.Contains('A')).ToList();
                x = 1;
                foreach (string s in ls_A1)
                {
                    for (int i = 0; i <= r1; i++)
                    {
                        if (x - 1 == i)
                        {
                            Values[s].Add(1);
                        }
                        else
                        {
                            Values[s].Add(0);
                        }

                    }
                    x++;
                }
                DtTable.Columns.Add("Cj");
                foreach (string s in Names)
                {
                    DtTable.Columns.Add(s);
                }
                DtTable.Columns.Add("R.H.P");
                while (true)
                {

                    res = 0;

                    //get values
                    //str res
                    for (int i = 0; i <= r1; i++)
                    {
                        res += Cj[zj[i]] * RHP[i];
                    }


                    //end res
                    //start cj-zj;
                    foreach (string s in Names)
                    {
                        double val = 0;
                        for (int i = 0; i <= r1; i++)
                        {
                            val += Values[s][i] * Cj[zj[i]];
                        }
                        zj1[s] = val;
                    }
                    foreach (string s in Names)
                    {
                        cj_zj[s] = Cj[s] - zj1[s];
                    }
                    //end cj-zj
                    //Insert in Data Table
                    for (int i = 0; i <= r1; i++)
                    {

                        DataRow AddRow = DtTable.NewRow();
                        foreach (string s in Names)
                        {
                            AddRow[s] = Values[s][i];
                        }
                        AddRow["R.H.P"] = RHP[i];
                        AddRow["Cj"] = zj[i];
                        DtTable.Rows.Add(AddRow);
                    }
                    DataRow AddRow2 = DtTable.NewRow();
                    DataRow AddRow3 = DtTable.NewRow();

                    AddRow2["Cj"] = "zj";
                    AddRow2["R.H.P"] = res;
                    AddRow3["Cj"] = "Cj - zj";
                    foreach (string s in Names)
                    {
                        AddRow2[s] = zj1[s];
                        AddRow3[s] = cj_zj[s];
                    }

                    DtTable.Rows.Add(AddRow2);
                    DtTable.Rows.Add(AddRow3);
                    DtTable.Rows.Add();
                    DtTable.Rows.Add();

                    //end Insert in Data Table
                    //

                    string ma = returnMaxString(cj_zj);
                    if (ma == "" || cj_zj[ma] <= 0)
                    {
                        break;
                    }
                    List<double> Divs = Values[ma].ToList();

                    //
                    var RHP2 = RHP.ToList();
                    for (int i = 0; i <= r1; i++)
                    {
                        if (Values[ma][i] == 0)
                        {
                            RHP2[i] = -1;
                        }
                        else
                        {
                            RHP2[i] = RHP2[i] / Values[ma][i];
                        }
                    }
                    int mi = minRHP(RHP2);
                    if (RHP2[mi] <= 0)
                    {
                        break;
                    }
                    RHP[mi] = RHP2[mi];
                    zj[mi] = "";
                    zj[mi] = ma;
                    //div big number of this.row
                    double div = Values[ma][mi];
                    foreach (string s in Names)
                    {
                        Values[s][mi] = Values[s][mi] / div;
                    }
                    //s-[s[i]*r[i]]

                    for (int i = 0; i <= r1; i++)
                    {
                        if (i != mi)
                        {
                            div = Values[ma][i];
                            foreach (string s in Names)
                            {

                                Values[s][i] -= (div * Values[s][mi]);

                            }
                        }
                    }

                    for (int i = 0; i <= r1; i++)
                    {
                        if (i != mi)
                        {
                            RHP[i] -= (RHP2[mi] * Divs[i]);
                        }
                    }

                }
            }
            else

            {
                Dictionary<string, List<double>> Values = new Dictionary<string, List<double>>();
                Dictionary<string, double> Cj = new Dictionary<string, double>();
                int c1 = g1.ColumnCount - 1, c2 = g2.ColumnCount - 1, r1 = g1.RowCount - 2, r2 = g2.RowCount - 2;
                List<string> zj = new List<string>();
                Dictionary<string, double> zj1 = new Dictionary<string, double>();
                Dictionary<string, double> cj_zj = new Dictionary<string, double>();
                List<double> RHP = new List<double>();
                List<string> Names = new List<string>();

                //fill Cj from g2
                int x = 1;
                for (int j = 0; j <= c2; j++)
                {
                    int num = int.Parse(g2[j, 0].Value.ToString());
                    Names.Add($"x{x}");
                    Values.Add($"x{x}", new List<double>());
                    Cj.Add($"x{x++}", num);

                }

                //add R.h.p and Count Varible and Fill Cj from g1 and zj
                x = 1;
                int a = 1; for (int i = 0; i <= r1; i++)
                {
                    string s, A;
                    if (g1[c1 - 1, i].Value.ToString() == ">=")
                    {
                        s = $"S{x}"; A = $"A{a}";
                        //R.H.P ADD
                        RHP.Add(double.Parse(g1[c1, i].Value.ToString()));
                        //Cj Add
                        Cj.Add(s, 0);//s
                        Cj.Add(A, .5);//a
                                      //Add To Names
                        Names.Add(s);
                        Names.Add(A);
                        //names to value
                        Values.Add(s, new List<double>());
                        Values.Add(A, new List<double>());
                        //fill zj
                        zj.Add(A);
                        x++;
                        a++;

                    }
                    else if (g1[c1 - 1, i].Value.ToString() == "<=")
                    {
                        s = $"S{x}";
                        //R.H.P ADD
                        RHP.Add(double.Parse(g1[c1, i].Value.ToString()));
                        //Cj Add
                        Cj.Add(s, 0);//s
                                     // Cj.Add(A, -.5);//a
                                     //Add To Names
                        Names.Add(s);
                        // Names.Add(A);
                        //names to value
                        Values.Add(s, new List<double>());
                        //fill Zj
                        zj.Add(s);
                        x++;
                    }
                    else
                    {
                        A = $"A{a}";
                        //R.H.P ADD
                        RHP.Add(double.Parse(g1[c1, i].Value.ToString()));
                        //Cj Add
                        //Cj.Add(s, 0);//s
                        Cj.Add(A, .5);//a
                                      //Add To Names
                                      //Names.Add(s);
                        Names.Add(A);
                        //fill Value
                        Values.Add(A, new List<double>());
                        //fill ZJ
                        zj.Add(A);
                        a++;
                    }
                }

                //Fill Values form f2
                x = 1;
                for (int i = 0; i <= c1 - 2; i++)
                {
                    string v = $"x{x++}";
                    for (int j = 0; j <= r1; j++)
                    {
                        Values[v].Add(double.Parse(g1[i, j].Value.ToString()));
                    }
                }
                //Fill S
                List<string> ls_S = Names.Where(S => S.Contains('S') || S.Contains('A')).ToList();
                List<string> ls_S1 = Names.Where(S => S.Contains('S')).ToList();
                x = 1;
                foreach (string s in ls_S1)
                {
                    int val = 1;
                    var serch = ls_S.SingleOrDefault(ss => ss.Contains($"A{x}"));
                    if (serch != "" && serch != null)
                    {
                        val = -1;

                    }
                    for (int i = 0; i <= r1; i++)
                    {
                        if (x - 1 == i)
                        {
                            Values[s].Add(val);
                        }
                        else
                        {
                            Values[s].Add(0);
                        }
                    }
                    x++;
                }
                //Fill A
                List<string> ls_A1 = Names.Where(S => S.Contains('A')).ToList();
                x = 1;
                foreach (string s in ls_A1)
                {
                    for (int i = 0; i <= r1; i++)
                    {
                        if (x - 1 == i)
                        {
                            Values[s].Add(1);
                        }
                        else
                        {
                            Values[s].Add(0);
                        }

                    }
                    x++;
                }
                DtTable.Columns.Add("Cj");
                foreach (string s in Names)
                {
                    DtTable.Columns.Add(s);
                }
                DtTable.Columns.Add("R.H.P");

                while (true)
                {

                    res = 0;

                    //get values
                    //str res
                    for (int i = 0; i <= r1; i++)
                    {
                        res += Cj[zj[i]] * RHP[i];
                    }


                    //end res
                    //start cj-zj;
                    foreach (string s in Names)
                    {
                        double val = 0;
                        for (int i = 0; i <= r1; i++)
                        {
                            val += Values[s][i] * Cj[zj[i]];
                        }
                        zj1[s] = val;
                    }
                    foreach (string s in Names)
                    {
                        cj_zj[s] = Cj[s] - zj1[s];
                    }
                    //end cj-zj
                    //
                    //Insert in Data Table

                    for (int i = 0; i <= r1; i++)
                    {

                        DataRow AddRow = DtTable.NewRow();
                        foreach (string s in Names)
                        {
                            AddRow[s] = Values[s][i];
                        }
                        AddRow["R.H.P"] = RHP[i];
                        AddRow["Cj"] = zj[i];
                        DtTable.Rows.Add(AddRow);
                    }
                    DataRow AddRow2 = DtTable.NewRow();
                    DataRow AddRow3 = DtTable.NewRow();

                    AddRow2["Cj"] = "zj";
                    AddRow3["Cj"] = "Cj - zj";
                    AddRow2["R.H.P"] = res;
                    foreach (string s in Names)
                    {
                        AddRow2[s] = zj1[s];
                        AddRow3[s] = cj_zj[s];
                    }

                    DtTable.Rows.Add(AddRow2);
                    DtTable.Rows.Add(AddRow3);
                    DtTable.Rows.Add();
                    DtTable.Rows.Add();
                    //end Insert in Data Table
                    string ma = returnminTominString(cj_zj);
                    if (ma == "" || cj_zj[ma] >= 0)
                    {
                        break;
                    }
                    List<double> Divs = Values[ma].ToList();

                    //
                    var RHP2 = RHP.ToList();
                    for (int i = 0; i <= r1; i++)
                    {
                        if (Values[ma][i] == 0)
                        {
                            RHP2[i] = -1;
                        }
                        else
                        {
                            RHP2[i] = RHP2[i] / Values[ma][i];
                        }
                    }
                    int mi = minRHP(RHP2);
                    if (RHP2[mi] <= 0)
                    {
                        break;
                    }
                    RHP[mi] = RHP2[mi];
                    zj[mi] = "";
                    zj[mi] = ma;
                    //div big number of this.row
                    double div = Values[ma][mi];
                    foreach (string s in Names)
                    {
                        Values[s][mi] = Values[s][mi] / div;
                    }
                    //s-[s[i]*r[i]]

                    for (int i = 0; i <= r1; i++)
                    {
                        if (i != mi)
                        {
                            div = Values[ma][i];
                            foreach (string s in Names)
                            {

                                Values[s][i] -= (div * Values[s][mi]);

                            }
                        }
                    }

                    for (int i = 0; i <= r1; i++)
                    {
                        if (i != mi)
                        {
                            RHP[i] -= (RHP2[mi] * Divs[i]);
                        }
                    }

                }
            }
            return res;
        }
    }
}
