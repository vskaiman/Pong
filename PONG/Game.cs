﻿using System;
using VitPro;
using VitPro.Engine;

class Game : State
{
    public Camera cam = new Camera(240);

    public static Font font = new Font("./Fonts/arial.ttf", 25);

    static public Platform plat1 = new Platform(new Vec2(-100, -20), new Vec2(-80, 20), new Controller1());
    static public Platform plat2 = new Platform(new Vec2(100, -20), new Vec2(80, 20), new Controller2());

    static public Ball ball = new Ball(new Vec2(0, 0), 12);

    public override void Render()
    {
        cam.Apply();
        Draw.Clear(Color.Black);
        Draw.Rect(new Vec2(-150, -110), new Vec2(150, 110), Color.Green);
        Draw.Rect(new Vec2(-145, -105), new Vec2(145, 105), Color.White);
        ball.Render();
        plat1.Render();
        plat2.Render();

        Draw.Save();
        Draw.Translate(new Vec2(-30, 108));
        Draw.Scale(12);
        Draw.Color(Color.White);
        font.Render(plat1.score.ToString() + " : " + plat2.score.ToString());
        Draw.Load();
    }

    double t = 0;
    override public void Update(double dt)
    {
        if (isGoal)
        {
            t += dt;
        }
        else
        {
            ball.go(dt);
            world();
            meeting(plat1);
            meeting(plat2);
            goal();
        }

        plat1.Update(dt);
        plat2.Update(dt);

        if (t >= 2)
        {
            isGoal = false;
            t = 0;
        }

        if (Key.N.Pressed())
        {
            ball.pos = new Vec2(0, 0);
            ball.to = new Vec2(-1, 1);
        }
    }

    void meeting(Platform platform)
    {
        if (ball.pos.Y <= platform.pos2.Y &&
            ball.pos.Y >= platform.pos1.Y &&
            Math.Abs(ball.pos.X - platform.pos2.X) <= ball.radius)
        {

            ball.to = new Vec2(-platform.pos1.X / platform.pos1.Length, ball.to.Y);
            //ball.color = Color.Yellow;
        }
    }
    bool isGoal = false;
    void goal()
    {
        if (Math.Abs(ball.pos.X + 145) <= ball.radius ||
            Math.Abs(ball.pos.X - 145) <= ball.radius)
        {
            isGoal = true;

            if (ball.pos.X > 0)
            {
                plat1.score++;
            }
            else
            {
                plat2.score++;
            }

            ball.pos = Vec2.Zero;
        }
    }

    void world()
    {
        if (Math.Abs(ball.pos.Y + 105) <= ball.radius)
        {
            ball.to = new Vec2(ball.to.X, Math.Abs(ball.to.Y));
        }

        if (Math.Abs(ball.pos.Y - 105) <= ball.radius)
        {
            ball.to = new Vec2(ball.to.X, -Math.Abs(ball.to.Y));
        }

        if (Math.Abs(ball.pos.X + 145) <= ball.radius)
        {
            ball.to = new Vec2(Math.Abs(ball.to.X), ball.to.Y);
        }

        if (Math.Abs(ball.pos.X - 145) <= ball.radius)
        {
            ball.to = new Vec2(-Math.Abs(ball.to.X), ball.to.Y);
        }
    }
}