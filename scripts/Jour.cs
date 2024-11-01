using System;
using System. Collections;
using System. Collections. Generic;
using UnityEngine;



public class Jour
{
	// Attributs
	
	public readonly string nom;
	public readonly Creneau [] creneaux;
	public Transform element;
	
	
	// Constructeur
	
    public Jour
	(
		string nom,
		Creneau [] creneaux
	)
    {
        this. nom = nom;
        this. creneaux = creneaux;
    }

    
	// Affichage

    public override string ToString ()
	{
		return this. nom;
	}
}
