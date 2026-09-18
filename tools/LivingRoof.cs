using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
public class LivingRoof {
 static string[] hex={"#65a733","#95613e","#dedfd2","#319b9d","#658553","#354952","#f1cc64","#766258","#b6bebb"};
 static string[] verbs={"Folhas crescem e interceptam a chuva","Raízes se espalham e a água é retida","Partículas ficam. A água atravessa.","Reservatórios recebem água; o excesso escoa","As raízes encontram uma barreira de proteção","A água é conduzida pela superfície impermeável","A camada isolante reduz a passagem de calor","Proteção contra umidade no conjunto do sistema","A estrutura recebe o peso de todas as camadas"};
 static float a,scale,cx,cy;static double time;
 static Color Col(string h){return ColorTranslator.FromHtml(h);}
 static float Ease(double x){x=Math.Max(0,Math.Min(1,x));return (float)(x*x*(3-2*x));}
 static PointF P(float x,float z,float h){return new PointF(cx+scale*(x*(float)Math.Cos(a)-z*(float)Math.Sin(a)),cy+scale*((x*(float)Math.Sin(a)+z*(float)Math.Cos(a))*.38f-h));}
 static void Line(Graphics g,Color c,float w,PointF p,PointF q){using(var pen=new Pen(c,w)){pen.StartCap=LineCap.Round;pen.EndCap=LineCap.Round;g.DrawLine(pen,p,q);}}
 static void Fill(Graphics g,Color c,PointF[] pts){using(var b=new SolidBrush(c))g.FillPolygon(b,pts);}
 static void Text(Graphics g,string s,float size,float x,float y,float w,float h,Color color,bool bold){using(var f=new Font("Segoe UI",size,bold?FontStyle.Bold:FontStyle.Regular,GraphicsUnit.Pixel))using(var b=new SolidBrush(color))g.DrawString(s,f,b,new RectangleF(x,y,w,h));}
 static void Dot(Graphics g,Color c,float x,float y,float r){using(var b=new SolidBrush(c))g.FillEllipse(b,x-r,y-r,r*2,r*2);}
 static void Drop(Graphics g,PointF p,float radius){using(var b=new SolidBrush(Col("#42b9ed"))){g.FillEllipse(b,p.X-radius,p.Y-radius,radius*2,radius*2.4f);g.FillPolygon(b,new[]{new PointF(p.X-radius,p.Y),new PointF(p.X,p.Y-radius*2.5f),new PointF(p.X+radius,p.Y)});}Dot(g,Color.FromArgb(170,255,255,255),p.X-1,p.Y-1,radius*.32f);}
 static void Arrow(Graphics g,Color c,PointF p,PointF q,float width){using(var pen=new Pen(c,width)){pen.CustomEndCap=new AdjustableArrowCap(4,5);g.DrawLine(pen,p,q);}}
 static void Slab(Graphics g,int j,float height,float offset,float opacity,int active,double t,float grow){
  if(opacity<.01f)return;
  var state=g.Save();g.TranslateTransform(offset,0);
  int alpha=(int)(255*opacity);Color baseC=Col(hex[j]);Color top=Color.FromArgb(alpha,baseC);
  PointF[] poly={P(-345,-150,height),P(345,-150,height),P(345,150,height),P(-345,150,height)};
  Fill(g,Color.FromArgb(alpha,(int)(baseC.R*.60),(int)(baseC.G*.60),(int)(baseC.B*.60)),new[]{P(-345,150,height),P(345,150,height),P(345,150,height-15),P(-345,150,height-15)});
  Fill(g,Color.FromArgb(alpha,(int)(baseC.R*.8),(int)(baseC.G*.8),(int)(baseC.B*.8)),new[]{P(345,-150,height),P(345,150,height),P(345,150,height-15),P(345,-150,height-15)});
  Fill(g,top,poly);
  var random=new Random(j+522);
  if(j==1||j==7||j==8){for(int n=0;n<190;n++){PointF p=P(random.Next(-340,340),random.Next(-145,145),height+1);Dot(g,Color.FromArgb((int)(opacity*75),n%2==0?Color.White:Color.Black),p.X,p.Y,j==1?2:1);}}
  if(j==2||j==4||j==6){for(int n=-330;n<340;n+=24)Line(g,Color.FromArgb((int)(opacity*70),255,255,255),1,P(n,-150,height+1),P(n,150,height+1));for(int n=-150;n<=150;n+=24)Line(g,Color.FromArgb((int)(opacity*70),255,255,255),1,P(-345,n,height+1),P(345,n,height+1));}
  if(j==3){for(int x=-310;x<330;x+=54)for(int z=-115;z<140;z+=55){var p=P(x,z,height+2);using(var b=new SolidBrush(Color.FromArgb(alpha,22,100,112)))g.FillEllipse(b,p.X-16*scale,p.Y-6*scale,32*scale,12*scale);float wave=(float)(.6+.3*Math.Sin(t*2+x));using(var b=new SolidBrush(Color.FromArgb((int)(alpha*wave),104,213,233)))g.FillEllipse(b,p.X-11*scale,p.Y-3*scale,22*scale,6*scale);}}
  if(j==0){for(int n=0;n<125;n++){float x=random.Next(-330,330),z=random.Next(-140,140),h=(18+random.Next(24))*grow;PointF p=P(x,z,height),q=P(x+(float)Math.Sin(t*2+n)*7,z,height+h);Line(g,Color.FromArgb(alpha,43,107,41),2*scale,p,q);PointF mid=P(x,z,height+h*.55f);Fill(g,Color.FromArgb(alpha,97+random.Next(45),156+random.Next(35),57),new[]{mid,P(x-17,z-3,height+h*.85f),q,P(x+14,z+3,height+h*.7f)});if(n%9==0){for(int k=0;k<5;k++)Dot(g,Color.FromArgb(alpha,240,177+(n%3)*20,98+(n%4)*25),q.X+(float)Math.Cos(k*1.256+t*.3)*5,q.Y+(float)Math.Sin(k*1.256+t*.3)*5,3.5f);Dot(g,Color.FromArgb(alpha,253,224,91),q.X,q.Y,2.5f);}}}
  if(j==active){using(var pen=new Pen(Color.FromArgb((int)(opacity*(180+70*Math.Sin(t*3)*Math.Sin(t*3))),255,243,160),3)){g.DrawPolygon(pen,poly);}}
  g.Restore(state);
 }
 static void Action(Graphics g,int layer,float h,double t,float strength){
  if(strength<.1)return;
  if(layer==0){for(int n=0;n<26;n++){double phase=(t*.32+n*.079)%1;Drop(g,P(-290+(n*87)%590,-90+(n*53)%220,h+170-(float)phase*155),3.5f);}for(int n=0;n<3;n++){float x=(float)Math.Sin(t*.8+n*2)*260,z=(float)Math.Cos(t*.5+n)*120;PointF p=P(x,z,h+80+(float)Math.Sin(t*2)*14);using(var b=new SolidBrush(Col(n%2==0?"#e9a751":"#efd185"))){float wing=4+8*(float)Math.Abs(Math.Sin(t*10));g.FillEllipse(b,p.X-wing,p.Y-5,wing,10);g.FillEllipse(b,p.X,p.Y-5,wing,10);}}}
  else if(layer==1||layer==4){for(int n=0;n<10;n++){float x=-280+n*60;float length=Ease(t*.27-n*.04)*85;for(int k=0;k<5;k++){PointF from=P(x+(float)Math.Sin(k+n)*6,40,h+70-k*length/5),to=P(x+(float)Math.Sin(k+1+n)*10,40,h+70-(k+1)*length/5);if(layer==4){from=P(x+k*9,40,h+Math.Max(5,70-k*length/5));to=P(x+(k+1)*9,40,h+Math.Max(5,70-(k+1)*length/5));}Line(g,Col("#e6c399"),3,from,to);if(k>1)Line(g,Col("#dec59c"),1.5f,to,P(x+22,55,h+70-(k+1)*length/5));}}
   if(layer==1)for(int n=0;n<14;n++){float phase=(float)((t*.5+n*.13)%1);var p=P(-260+n*39,-70,h+60-phase*60);Drop(g,p,3);if(phase>.7){using(var pen=new Pen(Color.FromArgb((int)((1-phase)*350),40,151,193),2))g.DrawEllipse(pen,p.X-15*phase,p.Y-5,30*phase,10);}}
  }else if(layer==2){for(int n=0;n<22;n++){float phase=(float)((t*.45+n*.1)%1);float x=-300+(n*59)%600,z=-100+(n*31)%220;Drop(g,P(x,z,h+120-phase*210),3);PointF particle=P(x+14,z,h+Math.Max(3,90-phase*150));Dot(g,Col("#855d3b"),particle.X,particle.Y,4);}}
  else if(layer==3||layer==5||layer==7){for(int n=0;n<24;n++){float phase=(float)((t*.35+n*.078)%1);float x=-280+phase*570,z=-100+(n*41)%210;PointF p=P(x,z,h+8);Drop(g,p,3.5f);if(n%4==0)Arrow(g,Color.FromArgb(150,161,239,255),P(x-30,z,h+8),p,2);}if(layer==5||layer==7)for(int n=0;n<12;n++){float phase=(float)((t*.6+n*.12)%1);Drop(g,P(-260+n*48,10,h+90*(1-phase)),3);}}
  else if(layer==6){for(int n=0;n<10;n++){float x=-290+n*64;float offset=(float)((t*.7+n*.1)%1);for(int k=0;k<6;k++){float y=100-k*13;Line(g,Color.FromArgb(210,240,151,56),3,P(x+(float)Math.Sin(t*4+k)*8,0,h+y),P(x+(float)Math.Sin(t*4+k+1)*8,0,h+y-13));}Arrow(g,Col("#75b5c1"),P(x,0,h-23),P(x,0,h-34-offset*12),2);}}
  else if(layer==8){for(int n=0;n<6;n++){float x=-280+n*110;float dy=(float)Math.Sin(t*2+n)*9;Arrow(g,Col("#69897c"),P(x,0,h+100+dy),P(x,0,h+25),4);}for(int n=0;n<5;n++){float x=-270+n*135;Line(g,Col("#7f9b96"),10,P(x,80,h-20),P(x,80,h-90));}}
 }
 static void Frame(Graphics g,Image school,int active,string title,string detail,double t,double duration){
  time=t;float p=(float)(t/duration);float entrance=Ease(t/2.3);float isolate=Ease((p-.19)/.13)*(1-Ease((p-.77)/.13));float explode=Ease(t/3)*(1-Ease((p-.86)/.14));
  using(var sky=new LinearGradientBrush(new Rectangle(0,0,1280,720),Col("#c4e7f0"),Col("#f2f3db"),90))g.FillRectangle(sky,0,0,1280,720);
  for(int n=0;n<5;n++){float x=(float)((n*330+t*(10+n*2))%1650)-180;using(var b=new SolidBrush(Color.FromArgb(100,255,255,255))){g.FillEllipse(b,x,88+n%2*80,190,42);g.FillEllipse(b,x+40,61+n%2*80,85,63);}}
  using(var b=new SolidBrush(Color.FromArgb(80,247,217,123)))g.FillEllipse(b,1100,65,95,95);
  // A escola continua sendo a base visual do sistema.
  var schoolState=g.Save();float schoolScale=1-isolate*.22f;g.TranslateTransform(640,570+isolate*65);g.ScaleTransform(schoolScale,schoolScale);g.DrawImage(school,new Rectangle(-350,-55,700,153),new Rectangle(210,267,1170,380),GraphicsUnit.Pixel);g.Restore(schoolState);
  cx=630+(float)Math.Sin(t*.24)*22;cy=520+isolate*15;scale=.97f+isolate*.32f;a=.18f+(float)Math.Sin(t*.21-.8)*.22f;
  using(var b=new SolidBrush(Color.FromArgb(30,33,74,52)))g.FillEllipse(b,220,540,830,48);
  float activeH=0;
  for(int j=8;j>=0;j--){float h=55+(8-j)*(17+explode*22);float offset=0;float alpha=1;
   if(active>=0){h=h*(1-isolate)+(j==active?195:h)*isolate;if(j!=active){offset=(j%2==0?-1:1)*isolate*1150;alpha=1-isolate;}else activeH=h;}
   Slab(g,j,h,offset,alpha,active,t,.3f+.7f*Ease(t*.6));
  }
  if(active>=0)Action(g,active,activeH,t,Math.Max(.25f,isolate));
  else {Action(g,0,55+8*(17+explode*22),t,1);for(int n=0;n<8;n++){float phase=(float)((t*.4+n*.13)%1);Drop(g,new PointF(1060+phase*30,260+phase*260),4);}}
  // Título breve e legenda acompanham a ação de cada cena.
  using(var b=new SolidBrush(Color.FromArgb(235,19,64,47)))g.FillRectangle(b,0,0,1280,62);
  Text(g,"SPIN  /  O TELHADO GANHA VIDA",18,30,19,700,30,Color.White,true);
  for(int n=0;n<9;n++){Dot(g,n==active?Col("#f3d572"):Color.FromArgb(110,216,238,212),1000+n*26,31,n==active?6:3);}
  float slide=(1-Ease(t/.8))*60;
  Text(g,title,42,44+slide,89,1160,65,Col("#164c35"),true);
  string action=active>=0?(isolate>.7?verbs[active]:(p>.8?"Cada camada volta ao seu lugar no sistema":"A cobertura se abre: descubra o que existe por dentro")):"As nove camadas se abrem sobre a escola";
  using(var b=new SolidBrush(Color.FromArgb(238,255,254,240)))g.FillRectangle(b,28,582,1224,108);
  Text(g,action,27,49,597,1180,43,Col("#225c3d"),true);
  Text(g,detail,19,50,643,1150,35,Col("#52715e"),false);
  using(var b=new SolidBrush(Col("#77a946")))g.FillRectangle(b,28,687,1224*Math.Min(1,p),4);
  Text(g,"CORTE ILUSTRATIVO • SEM ESCALA",11,37,703,800,18,Col("#4d7160"),true);
 }
 public static void Render(string ffmpeg,string schoolPath,string output,int active,string title,string detail,double duration,string preview){
  var info=new ProcessStartInfo(ffmpeg,"-hide_banner -loglevel error -y -f rawvideo -pixel_format bgra -video_size 1280x720 -framerate 25 -i pipe:0 -an -c:v libx264 -preset veryfast -crf 21 -threads 2 -pix_fmt yuv420p \""+output+"\"");info.UseShellExecute=false;info.CreateNoWindow=true;info.RedirectStandardInput=true;
  using(var process=Process.Start(info))using(var photo=Image.FromFile(schoolPath))using(var bitmap=new Bitmap(1280,720,PixelFormat.Format32bppArgb)){
   byte[] bytes=new byte[1280*720*4];int frames=(int)Math.Round(duration*25);
   for(int frame=0;frame<frames;frame++){
    using(var g=Graphics.FromImage(bitmap)){g.SmoothingMode=SmoothingMode.AntiAlias;g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;Frame(g,photo,active,title,detail,frame/25.0,duration);}
    if(frame==(int)(frames*.5)&&!String.IsNullOrEmpty(preview))bitmap.Save(preview,ImageFormat.Png);
    var data=bitmap.LockBits(new Rectangle(0,0,1280,720),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);Marshal.Copy(data.Scan0,bytes,0,bytes.Length);bitmap.UnlockBits(data);process.StandardInput.BaseStream.Write(bytes,0,bytes.Length);
   }
   process.StandardInput.Close();process.WaitForExit();if(process.ExitCode!=0)throw new Exception("Falha no render");
  }
 }
}
