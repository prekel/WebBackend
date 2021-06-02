import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import RootComponent from './home.jsx';

import {Cart} from '../fsharp/Cart.js'
import {Carts} from '../fsharp/Carts.js'
import {Product} from '../fsharp/Product.js'
import {Products} from '../fsharp/Products.js'
import {Chat} from '../fsharp/Chat.js'
import {Chats} from '../fsharp/Chats.js'
import {Customer} from '../fsharp/Customer.js'
import {Operator} from '../fsharp/Operator.js'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.Components = {RootComponent, Cart, Carts, Product, Products, Chat, Chats, Customer, Operator};
