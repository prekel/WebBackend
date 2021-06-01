import { Product_ToDomain_Z6B097A11, Cart_ToDomain_268DEFC0 } from "./MyStore.Domain/Shop.js";
import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { some } from "./.fable/fable-library.3.1.11/Option.js";
import { printf, toText } from "./.fable/fable-library.3.1.11/String.js";
import { map as map_1, singleton, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { createElement } from "react";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export function Cart(cart) {
    const patternInput = [Cart_ToDomain_268DEFC0(cart.cart), map((arg00_1) => Product_ToDomain_Z6B097A11(arg00_1), cart.products)];
    const products = patternInput[1];
    const cart_1 = patternInput[0];
    console.log(some(cart_1));
    console.log(some(products));
    const c = toText(printf("%A"))(cart_1);
    const p = toText(printf("%A"))(products);
    const children = toList(delay(() => append(singleton(createElement("p", {
        children: [c],
    })), delay(() => append(singleton(createElement("p", {
        children: [p],
    })), delay(() => map_1((i) => createElement("p", {
        children: [i.Description],
    }), products)))))));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

