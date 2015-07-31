
 
 
 




declare module Store.DTOs {
	interface CheckoutIndex {
		deliveryAddress: Store.DTOs.Address;
		billingAddress: Store.DTOs.Address;
		lineItems: Store.DTOs.LineItem[];
		countries: Store.DTOs.Country[];
		subTotal: string;
		tax: string;
		shippingCosts: string;
		total: string;
		checkoutShippingCostsUrl: string;
	}
	interface CheckoutVariableCosts {
		shippingCosts: string;
		tax: string;
		total: string;
	}
}
declare module Store.DTOs {
	interface Address {
		salutation: string;
		firstname: string;
		lastname: string;
		address1: string;
		address2: string;
		zip: string;
		city: string;
		countryId: string;
		email: string;
	}
}
declare module Store.DTOs {
	interface LineItem {
		name: string;
		qty: number;
		price: number;
		subtotal: number;
	}
	interface Country {
		id: string;
		name: string;
	}
}


declare module Store.DTOs.Ko {
	interface CheckoutIndex {
		deliveryAddress: Store.DTOs.Ko.Address;
		billingAddress: Store.DTOs.Ko.Address;
		lineItems: KnockoutObservableArray<Store.DTOs.Ko.LineItem>;
		countries: KnockoutObservableArray<Store.DTOs.Ko.Country>;
		subTotal: KnockoutObservable<string>;
		tax: KnockoutObservable<string>;
		shippingCosts: KnockoutObservable<string>;
		total: KnockoutObservable<string>;
		checkoutShippingCostsUrl: KnockoutObservable<string>;
	}
	interface CheckoutVariableCosts {
		shippingCosts: KnockoutObservable<string>;
		tax: KnockoutObservable<string>;
		total: KnockoutObservable<string>;
	}
}
declare module Store.DTOs.Ko {
	interface Address {
		salutation: KnockoutObservable<string>;
		firstname: KnockoutObservable<string>;
		lastname: KnockoutObservable<string>;
		address1: KnockoutObservable<string>;
		address2: KnockoutObservable<string>;
		zip: KnockoutObservable<string>;
		city: KnockoutObservable<string>;
		countryId: KnockoutObservable<string>;
		email: KnockoutObservable<string>;
	}
}
declare module Store.DTOs.Ko {
	interface LineItem {
		name: KnockoutObservable<string>;
		qty: KnockoutObservable<number>;
		price: KnockoutObservable<number>;
		subtotal: KnockoutObservable<number>;
	}
	interface Country {
		id: KnockoutObservable<string>;
		name: KnockoutObservable<string>;
	}
}





