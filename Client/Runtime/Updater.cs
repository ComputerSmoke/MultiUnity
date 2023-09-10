using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiunity.Shared;
using System.Diagnostics;
using System;

namespace Multiunity.Unity {
    public class Updater : MonoBehaviour
    {
        long prevUpdate;
        long prevCheck;
        Stopwatch timer;
        Entity prev;
        float margin;
        float marginRot;
        // Start is called before the first frame update
        void Start()
        {
            margin = .1F;
            marginRot = .1F;
            prev = Encoder.Encode(gameObject);
            timer = new();
            timer.Start();
        }

        void FixedUpdate()
        {
            if(timer.ElapsedMilliseconds - prevCheck < 100) return;
            prevCheck = timer.ElapsedMilliseconds;
            Entity entity = Encoder.Encode(gameObject);
            if(!Jerk(entity)) return;
            prev = entity;
            prevUpdate = timer.ElapsedMilliseconds;
            MultiSession.Update(gameObject);
        }
        //true if object is not within margin of predicted location by last update.
        //TODO: replace this calculation with actual simulation in a shadow layer.
        private bool Jerk(Entity entity) {
            float delta = ((float)(timer.ElapsedMilliseconds - prevUpdate))/1000;
            (float px, float py) = prev.pos; 
            (float vx, float vy) = prev.vel;
            (float ax, float ay) = prev.accel;
            float expectedX = calcExpected(px, vx, ax, prev.drag, delta);
            float expectedY = calcExpected(py, vy, ay, prev.drag, delta);
            float expectedRot = calcExpected(prev.rot, prev.rotv, prev.rota, prev.angularDrag, delta);
            expectedRot = expectedRot % ((float)(2*Math.PI));
            if(expectedRot < 0) 
                expectedRot += (float)(2*Math.PI);
            return Math.Abs(entity.PX() - expectedX) > margin || Math.Abs(entity.PY() - expectedY) > margin || Math.Abs(entity.rot - expectedRot) > marginRot;
        }
        private float calcExpected(float x, float v, float a, float d, float t) {
            if(d == 0) return x + v*t + a*t*t/2;
            //If drag greater than 0, we use a formula derived from (x'') = (a)-(d)(x') where d is drag and a is our would-be acceleration without drag.
            return (a*t - (a/d - v)*(1-(float)Math.Exp(-d*t)))/d + x;
        }
    }
}