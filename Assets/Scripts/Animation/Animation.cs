using UnityEngine;

public class Animation : MonoBehaviour
{
    
    private Animator viktorsAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        viktorsAnimator = GetComponent<Animator>();

        viktorsAnimator.SetBool("Walk", false);

        viktorsAnimator.SetBool("Jump", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            viktorsAnimator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp("d"))
        {
            viktorsAnimator.SetBool("Walk", false);
        }
        
        
        if (Input.GetKeyDown("a"))
        {
            viktorsAnimator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp("a"))
        {
            viktorsAnimator.SetBool("Walk", false);
        }

        
        if (Input.GetButtonDown("Jump")) 
        {
            viktorsAnimator.SetBool("Jump", true);
        }

        if (Input.GetButtonUp("Jump")) 
        {
            viktorsAnimator.SetBool("Jump", false);
        }
    }
}
