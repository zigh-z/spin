using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
public class RoofMovie {
 static Color C(string hex){return ColorTranslator.FromHtml(hex);}
 static string[] colors={"#67a53e","#926039","#dddcc5","#39969c","#667b5b","#303f48","#e2c56d","#655348","#aab0ae"};
 static string[] names={"Vegetação","Substrato","Camada filtrante","Camada drenante","Manta protetora","Membrana impermeável","Isolante térmico","Camada contra umidade","Laje e estrutura"};
 static string[] materials={"Plantas de baixo porte","Mistura preparada para plantio","Geotêxtil não tecido","Geocomposto drenante","Proteção contra raízes","Membrana EPDM","Placas de XPS","Manta asfáltica","Base estrutural"};
 static void Txt(Graphics g,string s,float size,float x,float y,float w,float h,Color color,bool bold){using(var f=new Font("Segoe UI",size,bold?FontStyle.Bold:FontStyle.Regular,GraphicsUnit.Pixel))using(var b=new SolidBrush(color))g.DrawString(s,f,b,new RectangleF(x,y,w,h));}
 static void Rect(Graphics g,Color c,float x,float y,float w,float h){using(var b=new SolidBrush(c))g.FillRectangle(b,x,y,w,h);}
 static PointF[] Top(float x,float y){return new[]{new PointF(x,y),new PointF(x+470,y),new PointF(x+640,y-78),new PointF(x+170,y-78)};}
 static void Poly(Graphics g,Color c,PointF[] p){using(var b=new SolidBrush(c))g.FillPolygon(b,p);}
 static void Drop(Graphics g,float x,float y,float alpha){using(var b=new SolidBrush(Color.FromArgb((int)(alpha*210),54,166,219))){g.FillEllipse(b,x-3,y,7,11);g.FillPolygon(b,new[]{new PointF(x-3,y+3),new PointF(x+1,y-7),new PointF(x+4,y+3)});}}
 static void Frame(Graphics g,Image school,int active,string title,string detail,double time){
  using(var sky=new LinearGradientBrush(new Rectangle(0,0,1280,720),C("#d8edf4"),C("#f5f5e7"),90))g.FillRectangle(sky,0,0,1280,720);
  using(var b=new SolidBrush(Color.FromArgb(120,255,255,255))){float cloud=50+(float)Math.Sin(time*Math.PI/3)*15;g.FillEllipse(b,cloud,70,160,45);g.FillEllipse(b,cloud+60,45,90,65);g.FillEllipse(b,550-cloud,62,150,38);}
  Rect(g,C("#194e3a"),0,0,1280,60);Txt(g,"SPIN  /  UMA VIAGEM PELO TELHADO VERDE",21,32,17,1000,36,Color.White,true);
  Txt(g,"CORTE EM PERSPECTIVA • CAMADAS SEPARADAS PARA VISUALIZAÇÃO",13,40,77,850,30,C("#416653"),true);
  using(var sh=new SolidBrush(Color.FromArgb(30,18,45,33)))g.FillEllipse(sh,50,630,740,42);
  g.DrawImage(school,new Rectangle(66,508,700,147),new Rectangle(210,267,1170,380),GraphicsUnit.Pixel);
  Rect(g,C("#174e78"),66,508,700,43);Txt(g,"SPIN EDUCACIONAL",29,248,511,450,50,Color.White,true);
  float pulse=(float)(.5-.5*Math.Cos(time*Math.PI/3));
  float[] ys=new float[9];float[] xs=new float[9];
  for(int j=8;j>=0;j--){
   float y=206+j*34;float x=91;
   if(active<0){y-= (8-j)*pulse*2;}
   else {if(j<=active)y-=18; if(j==active)x-=18+8*pulse;}
   ys[j]=y;xs[j]=x;Color color=C(colors[j]);
   Poly(g,ControlDark(color),new[]{new PointF(x,y),new PointF(x+470,y),new PointF(x+470,y+14),new PointF(x,y+14)});
   Poly(g,Color.FromArgb((int)(color.R*.8),(int)(color.G*.8),(int)(color.B*.8)),new[]{new PointF(x+470,y),new PointF(x+640,y-78),new PointF(x+640,y-64),new PointF(x+470,y+14)});
   var poly=Top(x,y);Poly(g,color,poly);
   using(var path=new GraphicsPath()){path.AddPolygon(poly);var state=g.Save();g.SetClip(path);var random=new Random(220+j);
    if(j==1||j==7||j==8){for(int n=0;n<350;n++){int px=random.Next((int)x,(int)x+640);int py=random.Next((int)y-78,(int)y+1);using(var b=new SolidBrush(Color.FromArgb(80,n%2==0?Color.White:Color.Black)))g.FillEllipse(b,px,py,j==1?3:2,2);}}
    if(j==2||j==4||j==6){using(var p=new Pen(Color.FromArgb(50,255,255,255),1)){for(int n=0;n<55;n++)g.DrawLine(p,x+n*15,y+10,x+n*15+170,y-85);for(int n=0;n<8;n++)g.DrawLine(p,x,y-n*12,x+640,y-n*12);}}
    if(j==3){using(var b=new SolidBrush(C("#226d76")))for(int n=0;n<80;n++)g.FillEllipse(b,x+random.Next(630),y-random.Next(78),12,5);}
    g.Restore(state);
   }
   if(j==0){var random=new Random(41);for(int n=0;n<170;n++){float u=(float)random.NextDouble(),v=(float)random.NextDouble();float px=x+u*460+v*166;float py=y-v*74;float sway=(float)Math.Sin(time*Math.PI/3+n)*3;using(var p=new Pen(n%2==0?C("#276b36"):C("#96bd4b"),2)){g.DrawLine(p,px,py,px+sway-4,py-12-(n%8));g.DrawLine(p,px,py,px+sway+5,py-10);}if(n%17==0)using(var b=new SolidBrush(C("#eab870")))g.FillEllipse(b,px+sway-4,py-19,6,6);}}
   if(j==active){using(var p=new Pen(Color.FromArgb(180+(int)(pulse*75),255,255,231),3+pulse*2))g.DrawPolygon(p,poly);}
   using(var b=new SolidBrush(j==active?C("#fbf1ae"):C("#ffffff")))g.FillEllipse(b,34,y-8,30,30);
   Txt(g,(j+1).ToString(),16,42,y-5,25,27,C("#24543c"),true);
  }
  // A chuva atravessa as camadas superiores; o excesso segue pela drenagem.
  for(int n=0;n<22;n++){double phase=(time/2+n*.137)%1;float x=150+(n*89)%480;float y=110+(float)phase*250;Drop(g,x,y,y>ys[3]?0.2f:0.8f);}
  using(var pipe=new Pen(C("#849798"),16)){pipe.LineJoin=LineJoin.Round;g.DrawLines(pipe,new[]{new PointF(730,ys[3]-26),new PointF(758,ys[3]-18),new PointF(777,565),new PointF(796,580)});}
  using(var pipe=new Pen(C("#71c6e2"),8)){pipe.LineJoin=LineJoin.Round;g.DrawLines(pipe,new[]{new PointF(730,ys[3]-26),new PointF(758,ys[3]-18),new PointF(777,565),new PointF(796,580)});}
  for(int n=0;n<5;n++){float phase=(float)((time/2+n*.2)%1);Drop(g,759+phase*18,ys[3]+phase*(555-ys[3]),.8f);}
  Rect(g,Color.FromArgb(247,255,254,242),816,102,428,507);
  Txt(g,active>=0?"CAMADA "+(active+1).ToString("00")+" / 09":"TODAS AS CAMADAS JUNTAS",16,842,126,375,35,C("#5b873d"),true);
  Txt(g,title,35,840,175,373,122,C("#1a5037"),true);
  Txt(g,active>=0?materials[active]:"Um sistema vivo sobre a escola",18,843,298,356,63,C("#658148"),true);
  Txt(g,detail,25,840,382,370,154,C("#426351"),false);
  Txt(g,"CHUVA → RETENÇÃO → DRENAGEM",13,842,563,375,28,C("#357989"),true);
  if(active>=0){float anchor=ys[active]-27;using(var pen=new Pen(C("#3f7855"),2)){g.DrawLine(pen,xs[active]+610,anchor,795,anchor);g.DrawLine(pen,795,anchor,816,252);}using(var b=new SolidBrush(C("#f7df77")))g.FillEllipse(b,xs[active]+603,anchor-6,12,12);}
  Rect(g,C("#194e3a"),0,669,1280,51);Txt(g,"PROPOSTA CONCEITUAL • SEM ESCALA",13,32,686,620,27,C("#e2efd1"),true);Txt(g,"Escola SPIN · Rio das Ostras / RJ",16,870,683,390,30,Color.White,false);
 }
 static Color ControlDark(Color c){return Color.FromArgb((int)(c.R*.6),(int)(c.G*.6),(int)(c.B*.6));}
 public static void Render(string ffmpeg,string schoolPath,string output,int active,string title,string detail,string preview){
  var info=new ProcessStartInfo(ffmpeg,"-hide_banner -loglevel error -y -f rawvideo -pixel_format bgra -video_size 1280x720 -framerate 25 -i pipe:0 -an -c:v libx264 -preset veryfast -crf 22 -threads 2 -pix_fmt yuv420p \""+output+"\"");info.UseShellExecute=false;info.CreateNoWindow=true;info.RedirectStandardInput=true;
  using(var process=Process.Start(info))using(var photo=Image.FromFile(schoolPath))using(var bitmap=new Bitmap(1280,720,PixelFormat.Format32bppArgb)){
   byte[] bytes=new byte[1280*720*4];
   for(int frame=0;frame<150;frame++){
    using(var g=Graphics.FromImage(bitmap)){g.SmoothingMode=SmoothingMode.AntiAlias;g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;Frame(g,photo,active,title,detail,frame/25.0);}
    if(frame==45&&!String.IsNullOrEmpty(preview))bitmap.Save(preview,ImageFormat.Png);
    var data=bitmap.LockBits(new Rectangle(0,0,1280,720),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);Marshal.Copy(data.Scan0,bytes,0,bytes.Length);bitmap.UnlockBits(data);process.StandardInput.BaseStream.Write(bytes,0,bytes.Length);
   }
   process.StandardInput.Close();process.WaitForExit();if(process.ExitCode!=0)throw new Exception("Falha no render");
  }
 }
}
