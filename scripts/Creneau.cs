using System;
using System. Collections;
using System. Collections. Generic;
using UnityEngine;



public class Creneau
{
	// Attributs
	
	public readonly string heure;
	public readonly string minute;
	public Transform element;
	
	
	// Constructeurs
	
    public Creneau
	(
		string heure,
		string minute
	)
    {
        this. heure = heure;
		this. minute = minute;
    }
	
	// L'on repasse l'heure en string dans le cas où un int est donné
	public Creneau
	(
		int heure,
		string minute
	)
    {
        this. heure = heure. ToString ();
		this. minute = minute;
    }

    
	// Affichage

    public override string ToString ()
	{
		return
			this. heure. ToString ()
			+ ":"
			+ this. minute. ToString ()
		;
	}
}
