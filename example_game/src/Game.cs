using System;
using System.Collections.Generic;
using LibNet.Sharp2D;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

public class Game : GameWindow
{
    private Renderer Graphics;
    private Player Player;

    private RenderImage FloorImage;
    private RenderImage CactoImage;

    private RenderText HowPlay;
    private Vector2 HowPlayPos;
    private RenderText ScoreText;

    private List<RectPosition> Floor = new List<RectPosition>();
    private List<RectPosition> Cactos = new List<RectPosition>(12);


    private float velocidade = 4;
    private int JumpHeight = 200;

    private float acel = 0;


    private float Score = 0;
    private bool Jump = false;
    private bool OnFloor = true;

    private float distLast = 0;
    private bool Alive = true;
    public Game() : base(800, 640, GraphicsMode.Default, "Game Example")
    {
        Graphics = new Renderer(800, 640);

        Player = new Player()
        {
            transform = new RectPosition(new Vector2(300, 540), new Vector2(50, 50)),
            image = new RenderImage(Renderer.LoadImage("./images/player.png"))
        };

        FloorImage = new RenderImage(Renderer.LoadImage("./images/chao.png"));
        CactoImage = new RenderImage(Renderer.LoadImage("./images/cacto.png"));

        for (int i = 0; i < 20; i++)
        {
            Floor.Add(new RectPosition(new Vector2(i * 50, 590), new Vector2(50, 50)));
        }

    }

    protected override void OnLoad(System.EventArgs e)
    {
        HowPlay = new RenderText("Press any button to jump!", 35);
        HowPlay.TextColor = SColor.Yellow;
        HowPlayPos = new Vector2(400 - HowPlay.MeasureSize().X / 2, 100);
        ScoreText = new RenderText(Score.ToString(), 20);
        VSync = VSyncMode.On;
    }
    protected override void OnResize(System.EventArgs e)
    {
        this.Width = 800;
        this.Height = 640;
        Graphics.Resize(800, 640);
    }
    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {

        if (OnFloor == true)
        {
            Jump = true;
            OnFloor = false;
        }
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        if (!Alive) return;
        Score += (float)e.Time;
        ScoreText.Text = "Score: " + ((int)Score).ToString();

        if (Jump) acel += 0.025f;
        if (Jump == false && Player.transform.position.Y >= 540)
        {
            OnFloor = true;
            acel = 0;
        }
        if (Jump == true && Player.transform.position.Y > 540 - JumpHeight) Player.transform.position.Y -= 10;
        if (Jump == true && Player.transform.position.Y <= 540 - JumpHeight) Jump = false;
        if (Jump == false && OnFloor == false) Player.transform.position.Y += 4 + acel;



        for (int i = 0; i < Floor.Count; i++)
        {
            var f = Floor[i];
            f.position.X -= velocidade;
            if (f.position.X <= -50)
            {
                f.position.X = 19 * 50;
            }
            Floor[i] = f;
        }
        distLast += velocidade;

        if (distLast > 250)
        {
            distLast = 0;
            int n = new Random().Next() % 100;
            if (n < 15)
            {
                Cactos.Add(new RectPosition(new Vector2(850, 540), new Vector2(50, 50)));
            }
            else if (n < 20)
            {
                Cactos.Add(new RectPosition(new Vector2(850, 540), new Vector2(50, 50)));
                Cactos.Add(new RectPosition(new Vector2(900, 540), new Vector2(50, 50)));
                distLast -= 50;
            }
            else if (n < 23)
            {
                Cactos.Add(new RectPosition(new Vector2(850, 540), new Vector2(50, 50)));
                Cactos.Add(new RectPosition(new Vector2(900, 540), new Vector2(50, 50)));
                Cactos.Add(new RectPosition(new Vector2(950, 540), new Vector2(50, 50)));
                distLast -= 75;
            }
        }

        for (int i = 0; i < Cactos.Count; i++)
        {
            var f = Cactos[i];
            f.position.X -= velocidade;
            if (f.position.X <= -50)
            {
                Cactos.Remove(f);
            }
            else
                Cactos[i] = f;
        }


        foreach (var c in Cactos)
        {
            if (RectPosition.Hit(Player.transform, c))
            {
                HowPlay.Text = "You Loose!";
                Alive = false;
                HowPlayPos.Y = 300;
                HowPlayPos.X = 400 - HowPlay.MeasureSize().X / 2;
            }
        }

    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        GL.ClearColor(SColor.CornflowerBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        if (Alive)
        {
            Graphics.Draw(Player.image, Player.transform);
            Graphics.Draw(FloorImage, Floor.ToArray());
            Graphics.Draw(CactoImage, Cactos.ToArray());
            Graphics.Draw(HowPlay, HowPlayPos, HowPlay.MeasureSize());
            Graphics.Draw(ScoreText, new Vector2(10, 10), ScoreText.MeasureSize());
        }
        else
        {
            Graphics.Draw(HowPlay, HowPlayPos, HowPlay.MeasureSize());
            Graphics.Draw(ScoreText, new Vector2(400 - ScoreText.MeasureSize().X / 2, HowPlayPos.Y + 50), ScoreText.MeasureSize());
        }
        Context.SwapBuffers();

    }
}