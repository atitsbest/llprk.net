
 
 
 




declare module DTOs.Tax {
	interface TaxIndex {
		countries: DTOs.Tax.Country[];
		changeCountryTaxUrl: string;
	}
	interface Country {
		id: string;
		name: string;
		taxId: number;
		taxPercent: number;
	}
}


declare module DTOs.Tax.Ko {
	interface TaxIndex {
		countries: KnockoutObservableArray<DTOs.Tax.Ko.Country>;
		changeCountryTaxUrl: KnockoutObservable<string>;
	}
	interface Country {
		id: KnockoutObservable<string>;
		name: KnockoutObservable<string>;
		taxId: KnockoutObservable<number>;
		taxPercent: KnockoutObservable<number>;
	}
}





