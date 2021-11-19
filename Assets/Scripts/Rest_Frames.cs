using UnityEngine;
using System.Collections;
using System.IO;

public class Rest_Frames : MonoBehaviour
{
    ParticleSystem particleSystem;                 
    ParticleSystem.Particle[] allParticles;         
    int pointCount;                                 
    public GameObject parent;
    public GameObject anchor;
    public float density = 1;
    public double angularSize = 0.0F;
    private float radius = 2.0f;
    [Range(0.0F, 1.0F)]
    public float transparency;
    ArrayList arrayListXYZ = new ArrayList();
    public void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        arrayListXYZ.Clear();
        CalualteSphere();
        double size2 = 2*radius*System.Math.Sin((angularSize * System.Math.PI)/ 360.0);
        anchor.transform.parent = parent.transform;
        particleSystem.startSpeed = 0.0F;                           
        particleSystem.startLifetime = 1000.0F;

        pointCount = arrayListXYZ.Count;
        allParticles = new ParticleSystem.Particle[pointCount];
        particleSystem.maxParticles = pointCount;                   
        particleSystem.Emit(pointCount);                           
        particleSystem.GetParticles(allParticles);                  
        for (int i = 0; i < pointCount; i++)
        {
            allParticles[i].position = (Vector3)arrayListXYZ[i];
            allParticles[i].startColor = new Color(0, 0, 0, 1.0F - transparency);
            allParticles[i].startSize = (float)size2;                      
        }

        particleSystem.SetParticles(allParticles, pointCount);
        var main = particleSystem.main;
        main.startColor = new Color(0, 0, 0, transparency);
    }

    void Update()
    {
    }

    void CalualteSphere()
    {
        int n = (int)(180 * 360 * density);
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2.0F/ n;
        float x = 0;
        float y = 0;
        float z = 0;
        float r = 0;
        float phi = 0;

        for (var k = 0; k < n; k++)
        {
            float k1 = Random.Range(k * 1.0F, (k + 1) * 1.0F);
            y = k1 * off - 1 + (off / 2);
            r = Mathf.Sqrt(1 - y * y);
            phi = Random.Range(k * inc*1.0F, (k+0.1F) * inc * 1.0F);
            x = Mathf.Cos(phi) * r;
            z = Mathf.Sin(phi) * r;
            if (float.IsNaN(x * y * z))
            {
                k -= 1;
                continue;
            }
            else
            {
                arrayListXYZ.Add(new Vector3(x * radius, y * radius, z * radius));
            }
        }
    }
}