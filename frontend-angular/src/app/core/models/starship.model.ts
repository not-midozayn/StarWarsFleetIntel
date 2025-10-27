export interface Starship {
  id?: number;
  name: string;
  model: string;
  manufacturer: string;
  costInCredits: string;
  costConverted?: number;
  length: string;
  maxAtmospheringSpeed: string;
  crew: string;
  passengers: string;
  cargoCapacity: string;
  consumables: string;
  hyperdriveRating: string;
  mglt: string;
  starshipClass: string;
  currency?: string;
  pilots: string[];
  films: string[];
  specialModifications: string[];
  created: string; 
  edited: string;  
  preFlightCheck?: PreFlightCheckSummary;  
  url: string
}

export interface PreFlightCheckSummary {
  passed: boolean;
  warnings: string[];
  errors: string[];
}

export enum Currency {
  GalacticCredits = 'GalacticCredits',
  ImperialCredits = 'ImperialCredits',
  Wupiupi = 'Wupiupi',
  Truguts = 'Truguts',
  Peggats = 'Peggats'
}

export enum ModificationType {
  WeaponUpgrade = 'WeaponUpgrade',
  ShieldEnhancement = 'ShieldEnhancement',
  EngineBoost = 'EngineBoost',
  ArmorPlating = 'ArmorPlating',
  StealthSystem = 'StealthSystem'
}