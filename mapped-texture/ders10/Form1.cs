using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace ders10
{
    public partial class Form1 : Form
    {
        float alpha = 0, beta = 0;
        int bwidth, bheight;
        Bitmap curImage;

        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            reshape();
        }
        void reshape()
        {
            var filename = "brick1.bmp";
            var bmp = new Bitmap(filename);
            bwidth = bmp.Width;
            bheight = bmp.Height;
            var bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            Gl.glViewport(0, 0, bwidth, bheight);
            Gl.glClearColor(0.1f, 0.39f, 0.88f, 1);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60, 2, 1, 50);

            //60 degree specifies the field of view angle
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Glu.gluLookAt(2, -1, 5, 0, 0, 0, 0, 1, 0);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_TEXTURE_2D); //Doku Açma
            Gl.glTexImage2D(
                              Gl.GL_TEXTURE_2D,
                              0, // Level 0 level for MipMap(LOD)
                              Gl.GL_RGBA, // use R,G,B and Alpha components
                              bmp.Width, bmp.Height, // Texture width and height size
                              0, // border width
                              Gl.GL_BGR_EXT, //texels are in RGB format
                              Gl.GL_UNSIGNED_BYTE,
                              bmpData.Scan0
                              );
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0); // bind a named texture to a texturing target
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR); // adjust texel to the nearest pixel center 
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);

        }
        private void simpleOpenGlControl1_Load(object sender, EventArgs e)
        {

        }

        private void simpleOpenGlControl1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.D)
                alpha += 5;
            if (e.KeyCode == Keys.A)
                beta += 5;
            if (e.KeyCode == Keys.S)
                alpha -= 5;
            if (e.KeyCode == Keys.W)
                beta -= 5;
            simpleOpenGlControl1.Refresh();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glPushMatrix();
            Gl.glTranslatef(0, 0, -3);
            Gl.glRotatef(alpha, 1, 0, 0);
            Gl.glRotatef(beta, 0, 1, 0);
            display();
            Gl.glPopMatrix();
        }
        void display()
        {

            Gl.glBegin(Gl.GL_TRIANGLES);
            //left triangle
            Gl.glTexCoord2f(0.5f, 1f); Gl.glVertex2f(-3, 3);
            Gl.glTexCoord2f(0, 0); Gl.glVertex2f(-3, 0);
            Gl.glTexCoord2f(1, 0); Gl.glVertex2f(0, 0);
            //right triangle
            Gl.glTexCoord2f(4, 8); Gl.glVertex2f(3, 3);
            Gl.glTexCoord2f(0, 0); Gl.glVertex2f(0, 0);
            Gl.glTexCoord2f(8, 0); Gl.glVertex2f(3, 0);
            //bottom
            Gl.glTexCoord2f(1, 0.4f); Gl.glVertex2f(0, 0);
            Gl.glTexCoord2f(0.8f, 1); Gl.glVertex2f(-1.5f, -3);
            Gl.glTexCoord2f(0.3f, 0.4f); Gl.glVertex2f(1.5f, -3);

            Gl.glEnd();
        }

    }
}
