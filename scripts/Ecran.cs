using System;
using System. Collections;
using System. Collections. Generic;
using TMPro;
using UnityEngine;
using UnityEngine. UI;



public class Ecran: MonoBehaviour
{
	
	/*
	 *  Partie de la classe sur l'écran d'agenda
	 *  (voir plus bas pour la partie sur l'écran de création)
	 */
	
	
	// Constantes
	
	private const int
		nbJoursTravail = 6,
		nbHoraires = 26,
		horaireDebut = 8,
		largeurEcran = 1920,
		hauteurEcran = 1080,
		espaceCurseur = 17
	;
	private static readonly Color couleurDefaut = Couleur. gris;
	private static readonly string [] joursSemaine =
	{
		"Lundi",
		"Mardi",
		"Mercredi",
		"Jeudi",
		"Vendredi",
		"Samedi"
	};
	
	
	// Attributs
	
	private string enCours;
	private Sprite
		imageBordure,
		imageBordureInversee
	;
	private Jour [] edt;
	private int
		jourSelectionne,
		horaireSelectionne
	;
	private Transform
		menuAgenda,
		enTete,
		corps
	;
	private static float
		largeurEnTete,
		largeurCorps,
		hauteurEnTete,
		hauteurCorps
	;
	public static int
		largeurResolution,
		hauteurResolution
	;
	
	
	// Constructeur
	
	public Ecran ()
	{
		this. enCours = "principal";
		this. edt = new Jour [Ecran. nbJoursTravail];
		this. jourSelectionne    = -1;
		this. horaireSelectionne = -1;
		
		int heure;
		string minute = "";
		
		
		// Affectation des jours
		
		for
		(
			int numJour = 0;
			numJour < Ecran. nbJoursTravail;
			numJour ++
		)
		
		{
			this. edt [numJour] = new Jour (Ecran. joursSemaine [numJour], new Creneau [Ecran. nbHoraires]);
			
			
			// Affectation des créneaux pour chaque jour
			
			for
			(
				int index = 0;
				index < Ecran. nbHoraires;
				index ++
			)
			
			{
				// Affectation de l'heure
				heure = ((Ecran. horaireDebut * 2 + index) / 2);
				
				// Affectation de la minute
				switch (index % 2) 
				{
					case 0:
						minute = "00";
						break;
					case 1:
						minute = "30";
						break;
				}
				
				// Ajout à la liste du jour
				this. edt [numJour]. creneaux [index] = new Creneau (heure, minute);
			}
		}
	}
	
	
    // Start est appelée avant le rafraîchissement de la 1ère image
	
    void Start ()
    {
		// Chargement des images
		
		this. imageBordure         = Resources. Load <Sprite> ("bordure");
		this. imageBordureInversee = Resources. Load <Sprite> ("bordureInversee");
		
		
		// Chargement des éléments
		
		this. menuCreation = transform. Find ("creation");
		this. menuAgenda = transform. Find ("vueDefilement"). Find ("fenetre"). Find ("agenda");
		this. enTete = this. menuAgenda. Find ("enTete");
		this. corps  = this. menuAgenda. Find ("corps");
		Ecran. largeurEnTete = this. enTete. GetComponent <RectTransform> (). rect. width;
		Ecran. largeurCorps  = this. corps.  GetComponent <RectTransform> (). rect. width;
		Ecran. largeurResolution = Screen. currentResolution. width;
		Ecran. hauteurEnTete = this. enTete. GetComponent <RectTransform> (). rect. height;
		Ecran. hauteurCorps  = this. corps.  GetComponent <RectTransform> (). rect. height;
		Ecran. hauteurResolution = Screen. currentResolution. height;
		
		
		// Nous ajoutons les éléments de l'interface dans notre structure de données
		
		string
			nomJour,
			nomCreneau
		;
		
		foreach (Jour jour in this. edt)
		{
			nomJour =
				"contenu"
				+ jour. nom
			;
			jour. element = this. corps. Find (nomJour);
			
			foreach (Creneau creneau in jour. creneaux)
			{
				nomCreneau =
					"h"
					+ creneau. heure
					+ "min"
					+ creneau. minute
				;
				creneau. element = jour. element. Find (nomCreneau);
			}
		}
		
		
		// Tests
		
		string resultatsTests =
			"Sortie de l'agenda"
			+ "\n"
			+ this. edt [1]. creneaux [6]
			+ " "
			+ this. edt [1]. creneaux [6]. element
			+ "\n"
			+ this. edt [1]. creneaux [6]. element. Find ("description"). GetComponent <TMP_Text> (). isTextOverflowing
			+ "\n"
			+ this. edt [1]. creneaux [6]. element. Find ("description"). GetComponent <TMP_Text> (). isTextTruncated
			+ "\n"
			+ this. imageBordure
			+ "\n"
			+ Ecran. largeurEcran
			+ " x "
			+ Ecran. hauteurEcran
			+ "\n"
			+ Screen. currentResolution
		;
		
		Debug. Log (resultatsTests);
    }


    // Update est appelée à chaque image
	
    void Update ()
	{
		if (this. enCours == "principal" && this. jourSelectionne != -1)
		{
			if (Input. GetKeyDown (KeyCode. C) || Input. GetKeyDown (KeyCode. Return) || Input. GetKeyDown (KeyCode. KeypadEnter))
			{
				this. creer ();
			}
			
			else if (Input. GetKeyDown (KeyCode. S) || Input. GetKeyDown (KeyCode. Delete))
			{
				this. supprimer ();
			}
			
			else if (Input. GetKeyDown (KeyCode. D))
			{
				this. deplacer ();
			}
		}
	}
	
	
	// Ouvre le menu de création pour le créneau sélectionné
	
	public void creer ()
	{
		this. enCours = "creation";
		this. ecranCreation ();
	}
	
	
	// Supprime le créneau sélectionné s'il n'est pas déjà vide
	
	public void supprimer ()
	{
		Creneau creneauSelectionne = this. edt [this. jourSelectionne]. creneaux [this. horaireSelectionne];
		Transform elementSelectionne = creneauSelectionne. element;
		
		// S'il est non vide, l'on remet son tag à "Empty"
		if (elementSelectionne. tag != "Empty")
		{
			elementSelectionne. tag = "Empty";
			
			// On remet sa couleur de fond en gris
			Image cadre = elementSelectionne. GetComponent <Image> ();
			cadre. color = Ecran. couleurDefaut;
			
			// Et l'on efface les textes de l'horaire et de la description
			Transform
				elementHoraire     = elementSelectionne. Find ("horaire"),
				elementDescription = elementSelectionne. Find ("description")
			;
			elementHoraire.     GetComponent <TMP_Text> (). text = "";
			elementDescription. GetComponent <TMP_Text> (). text = "";
			
			// Retour de la console
			Debug. Log (creneauSelectionne + " supprimé");
		}
	}
	
	
	// Déplace le créneau sélectionné à la place d'un autre
	
	public void deplacer ()
	{
		this. enCours = "deplacement";

		// On inverse la luminosité de la bordure pour faire comprendre à l'utilisateur qu'il est en train de déplacer un créneau
		Creneau creneauSelectionne = this. edt [this. jourSelectionne]. creneaux [this. horaireSelectionne];
		Image cadre = creneauSelectionne. element. GetComponent <Image> ();
		cadre. sprite = this. imageBordureInversee;
	}
	
	
	// Effectue le déplacement de créneaux ordonné par l'utilisateur
	
	private void echanger ()
	{
		// On conserve les informations des deux créneaux dans des variables
		int
			jourDeplace    = this. jourSelectionne,
			horaireDeplace = this. horaireSelectionne
		;
		bordure ();
		Creneau
			creneauDeplace     = this. edt [jourDeplace].     creneaux [horaireDeplace],
			creneauSelectionne = this. edt [jourSelectionne]. creneaux [horaireSelectionne]
		;
		
		// On échange les tags des deux éléments
		(creneauDeplace. element. tag, creneauSelectionne. element. tag) =
		(creneauSelectionne. element. tag, creneauDeplace. element. tag);
		
		// On échange les visibilités des horaires sur les deux éléments
		TMP_Text
			texteHoraireDeplace     = creneauDeplace.     element. Find ("horaire"). GetComponent <TMP_Text> (),
			texteHoraireSelectionne = creneauSelectionne. element. Find ("horaire"). GetComponent <TMP_Text> ()
		;
		bool
			horaireDeplaceVisible     = texteHoraireDeplace.     text != "",
			horaireSelectionneVisible = texteHoraireSelectionne. text != ""
		;
		if (horaireDeplaceVisible)
		{
			texteHoraireSelectionne. text = creneauSelectionne. ToString ();
		}
		else
		{
			texteHoraireSelectionne. text = "";
		}
		if (horaireSelectionneVisible)
		{
			texteHoraireDeplace. text = creneauDeplace. ToString ();
		}
		else
		{
			texteHoraireDeplace. text = "";
		}
		
		
		// On échange les textes des descriptions des deux éléments
		TMP_Text
			descriptionDeplacee     = creneauDeplace.     element. Find ("description"). GetComponent <TMP_Text> (),
			descriptionSelectionnee = creneauSelectionne. element. Find ("description"). GetComponent <TMP_Text> ()
		;
		(descriptionDeplacee. text, descriptionSelectionnee. text) =
		(descriptionSelectionnee. text, descriptionDeplacee. text);
		
		// On échange les couleurs des deux éléments
		Image cadreDeplace     = creneauDeplace.     element. GetComponent <Image> ();
		Image cadreSelectionne = creneauSelectionne. element. GetComponent <Image> ();
		(cadreDeplace. color, cadreSelectionne. color) =
		(cadreSelectionne. color, cadreDeplace. color);
		
		this. enCours = "principal";
		
		// Retour de la console
		Debug. Log (creneauDeplace + " déplacé");
	}
	
	
	// Effectue une action lors d'un clic
	
	public void clicEnfonce ()
	{
		switch (this. enCours)
		{
			case "principal":
				this. bordure ();
				break;
				
			case "deplacement":
				this. echanger ();
				break;
		}
		
		// Retour de la console
		Debug. Log ("Clic enfoncé lors de l'état '" + this. enCours + "'.");
	}
	
	
	// Gère les bordures des créneaux sélectionnés
	
	public void bordure ()
	{
		Image cadre;
		
		// Enlèvement des bordures sur le créneau précédent
		if (this. jourSelectionne != -1)
		{
			cadre = this. edt [this. jourSelectionne]. creneaux [this. horaireSelectionne]. element. GetComponent <Image> ();
			cadre. sprite = null;
		}
		
		// Détection du nouveau créneau ciblé
		this. ciblage ();
		Creneau creneauSelectionne = this. edt [this. jourSelectionne]. creneaux [this. horaireSelectionne];
		
		// Ajoute des bordures sur le créneau ciblé
		cadre = creneauSelectionne. element. GetComponent <Image> ();
		cadre. sprite = this. imageBordure;
	}
	
	
	// À l'aide des coordonnées du pointeur, nous retrouvons le créneau ciblé
	
	private void ciblage ()
	{
		(float, float) coordonnees = this. formaterCoordonnees (Input. mousePosition);
		
		// Les parties entières des coordonnées nous donnent simplement les indices du créneau dans l'emploi du temps
		this. jourSelectionne    = (int) (coordonnees. Item1 * Ecran. nbJoursTravail);
		this. horaireSelectionne = (int) (coordonnees. Item2 * Ecran. nbHoraires);
	}
	
	
	// Reformate les coordonnées pour qu'elles soient utilisables et indépendantes de la taille de l'écran
	
	private (float, float) formaterCoordonnees
	(
		Vector3 vecteur
	)
	{
		// On normalise les coordonnées selon le corps de l'interface
		float x = vecteur. x * largeurEcran / largeurResolution;
		float y = vecteur. y * hauteurEcran / hauteurResolution;
		x -= this. menuAgenda. GetComponent <RectTransform> (). anchoredPosition. x;
		x = (x + (largeurCorps + espaceCurseur - largeurEcran) / 2) / largeurCorps;
		y = (y + (hauteurCorps - espaceCurseur - hauteurEcran + hauteurEnTete) / 2) / hauteurCorps;
		
		// On passe l'origine en haut à gauche
		y = 1 - y;
		
		// On s'assure que les coordonnées soient bien dans [0 ; 1[
		x = Calcul. contraindre (x, Ecran. nbJoursTravail);
		y = Calcul. contraindre (y, Ecran. nbHoraires);
		
		return (x, y);
	}
	
	
	
	/*
	 *  Partie de la classe sur l'écran de création
	 *  (voir plus haut pour la partie sur l'écran d'agenda)
	 */
	
	
	// Constantes
	
	private static readonly Color [] couleurPossibles =
	{
		Couleur. blanc,
		Couleur. rouge,
		Couleur. vert,
		Couleur. bleu,
		Couleur. cyan,
		Couleur. jaune,
		Couleur. magenta
	};
	
	
	// Attributs
	
	private bool averti;
	private string descriptionSelectionnee = "";
	private int
		horaireSelectionneDebut,
		horaireSelectionneFin
	;
	private Color couleurSelectionnee = Couleur. blanc;
	private Transform
		menuCreation,
		enTeteCreation,
		corpsCreation,
		avertissementCreation
	;
	private TMP_InputField entreeDescription;
	private TMP_Dropdown
		entreeDebut,
		entreeFin,
		entreeCouleur
	;
	
	
	// Initialise les attributs pour la fenêtre de création d'un créneau
	
	private void ecranCreation ()
	{
		// Chargement des éléments de la fenêtre
		this. menuCreation = transform. Find ("creation");
		this. enTeteCreation = menuCreation. Find ("enTete");
		this. corpsCreation  = menuCreation. Find ("corps");
		this. avertissementCreation = this. corpsCreation. Find ("avertissement");
		
		// Recherche du champ de texte pour la description
		this. entreeDescription = this. corpsCreation. Find ("description"). Find ("entree"). GetComponent <TMP_InputField> ();
		
		// Recherche de la liste déroulante pour l'horaire de début
		this. entreeDebut = this. corpsCreation. Find ("debut"). Find ("deroulant"). GetComponent <TMP_Dropdown> ();
		
		// Recherche de la liste déroulante pour l'horaire de fin
		this. entreeFin = this. corpsCreation. Find ("fin"). Find ("deroulant"). GetComponent <TMP_Dropdown> ();
		
		// Recherche de la liste déroulante pour la couleur
		this. entreeCouleur = this. corpsCreation. Find ("couleur"). Find ("deroulant"). GetComponent <TMP_Dropdown> ();
		
		// Valeurs par défaut des entrées
		this. menuCreation. gameObject. SetActive (true);
		this. horaireSelectionneDebut = horaireSelectionne;
		this. horaireSelectionneFin   = horaireSelectionne;
		this. entreeDebut. value = horaireSelectionne;
		this. entreeFin.   value = horaireSelectionne;
	}
	
	
	// Appelée après chaque fin de changement de la description
	
	public void majDescription ()
	{
		this. descriptionSelectionnee = "";
		string interdits = " \n\r";
		char precedent = ' ';
		
		// On remplace les sauts de ligne, et les espaces multiples par des espaces uniques
		foreach (char caractere in this. entreeDescription. text)
		{
			if (! Calcul. dans (caractere, interdits) || precedent != ' ')
			{
				switch (caractere)
				{
					case '\n' or '\r':
						precedent = ' ';
						break;
					
					default:
						precedent = caractere;
						break;
				}
				
				this. descriptionSelectionnee += precedent;
			}
		}
		
		this. entreeDescription. text = this. descriptionSelectionnee;
	}
	
	
	// Appelée après chaque changement des horaires de début et de fin
	
	public void majHoraires (bool estDebut)
	{
		// Mise à jour de l'horaire de début
		if (estDebut)
		{
			this. horaireSelectionneDebut = this. entreeDebut. value;
		}
		
		// Mise à jour de l'horaire de fin
		else
		{
			this. horaireSelectionneFin = this. entreeFin. value;
		}
		
		
		// Vérification des erreurs
		
		this. averti = false;
		
		// Erreur où l'horaire de début survient après celui de fin
		if (this. horaireSelectionneDebut > this. horaireSelectionneFin)
		{
			this. avertir ("L'horaire de début doit être avant celui de fin.");
		}
		
		// Erreur où les horaires voulus sont déjà pris par des créneaux existants
		else
		{
			for
			(
				int indiceEntre = horaireSelectionneDebut;
				indiceEntre <= horaireSelectionneFin;
				indiceEntre ++
			)
			{
				Transform elementEntre = this. edt [jourSelectionne]. creneaux [indiceEntre]. element;
				if (elementEntre. tag != "Empty")
				{
					this. avertir ("Il y a déjà un créneau à l'horaire choisi.");
					break;
				}
			}
		}
		
		if (! this. averti)
		{
			avertir ("");
			this. avertissementCreation. gameObject. SetActive (false);
		}
	}
	
	
	// Avertit l'utilisateur en cas de problème dans les choix effectués
	
	private void avertir (string message)
	{
		this. averti = true;
		this. avertissementCreation. gameObject. SetActive (true);
		
		TMP_Text sortieMessage = this. avertissementCreation. Find ("message"). GetComponent <TMP_Text> ();
		sortieMessage. text = message;
	}
	
	
	// Appelée après chaque changement de la couleur
	
	public void majCouleur ()
	{
		int indiceCouleur = this. entreeCouleur. value;
		this. couleurSelectionnee = Ecran. couleurPossibles [indiceCouleur];
	}
	
	
	// S'exécute après terminaison d'une création de créneau
	
	public void clicCreationTerminee (bool validee)
	{
		if (validee)
		{
			this. appliquerCreation ();
		}
		
		this. enCours = "principal";
		this. menuCreation. gameObject. SetActive (false);
	}
	
	
	// Effectue les modification choisies dans la fenêtre de création
	
	private void appliquerCreation ()
	{
		// On sépare la description pour pouvoir écrire les mots sur plusieurs lignes
		
		Queue <string> mots = new Queue <string> (this. descriptionSelectionnee. Split (" "));
		string ligne;
		TMP_Text zoneTexte = null;
		
		
		// Nous parcourons simplement tous les créneaux voulus entre l'horaire de début et celui de fin
		
		for
		(
			int indiceEntre = horaireSelectionneDebut;
			indiceEntre <= horaireSelectionneFin;
			indiceEntre ++
		)
		{
			Creneau creneauEntre = this. edt [jourSelectionne]. creneaux [indiceEntre];
			Transform elementEntre = creneauEntre. element;
			
			
			// On change son tag pour qu'il ne soit pas "Empty"
			
			elementEntre. tag = "Untagged";
			
			
			// On change sa couleur de fond
			
			Image cadre = elementEntre. GetComponent <Image> ();
			cadre. color = this. couleurSelectionnee;
			
			
			// On change les textes de l'horaire et de la description
			
			Transform
				elementHoraire     = elementEntre. Find ("horaire"),
				elementDescription = elementEntre. Find ("description")
			;
			
			if (indiceEntre == horaireSelectionneDebut)
			{
				// L'horaire est affiché pour la 1ère demi-heure
				elementHoraire. GetComponent <TMP_Text> (). text = creneauEntre. ToString ();
			}
			else
			{
				// Et vide pour les suivantes
				elementHoraire. GetComponent <TMP_Text> (). text = "";
			}
			
			// Gestion des débordements de la description
			zoneTexte = elementDescription. GetComponent <TMP_Text> ();
			if (mots. Count == 0)
			{
				zoneTexte. text = "";
			}
			else
			{
				zoneTexte. text = mots. Peek ();
				ligne =           mots. Peek ();
				if (zoneTexte. isTextOverflowing)
				{
					mots. Dequeue ();
				}
				while (! Calcul. texteDeborde (zoneTexte))
				{
					mots. Dequeue ();
					ligne = zoneTexte. text;
					if (mots. Count == 0)
					{
						break;
					}
					zoneTexte. text +=  " " + mots. Peek ();
				}
				zoneTexte. text = ligne;
			}
			
			
			// Retour de la console
			
			Debug. Log (creneauEntre + " créé");
		}
		
		
		/*
		 *  Dans le cas où des mots de la description seraient encore à placer,
		 *  on les rajoute simplement sur la dernière ligne
		 */
		
		if (zoneTexte != null)
		{
			foreach (string mot in mots)
			{
				zoneTexte. text += " " + mot;
			}
		}
	}
	
}
