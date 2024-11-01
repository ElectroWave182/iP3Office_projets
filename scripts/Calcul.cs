using System;
using System. Collections;
using System. Collections. Generic;
using TMPro;
using UnityEngine;
using UnityEngine. UI;



public static class Calcul
{
	// Contraint un réel dans l'intervalle semi-ouvert [0 ; 1[
	
	public static float contraindre
	(
		float reel,
		int precision
	)
	{
		// Borne inférieure fermée
		reel = Math. Max (reel, 0);
		
		// Borne supérieure ouverte
		reel = Math. Min (reel, 1 - 1f / (precision + 1));
		
		return reel;
	}
	
	
	// Vérifie si un caractère est dans une chaîne
	
	public static bool dans
	(
		char caractere,
		string chaine
	)
	{
		bool trouve = false;
		
		foreach (char element in chaine)
		{
			trouve = trouve || caractere == element;
			if (trouve)
			{
				break;
			}
		}
		
		return trouve;
	}
	
	
	/*
	 *  Recodage de la fonction "isTextOverflowing" qui retourne false tout le temps :
	 *  retourne un booléen disant si la largeur du texte dépasse celle de son rectangle
	 */
	
    public static bool texteDeborde (TMP_Text zoneTexte)
    {
		RectTransform rectangle = zoneTexte. rectTransform;
        float largeurTexte = LayoutUtility. GetPreferredWidth (rectangle);
        float largeurRectangle = rectangle. rect. width; 
		
        return largeurTexte > largeurRectangle;
    }
}
