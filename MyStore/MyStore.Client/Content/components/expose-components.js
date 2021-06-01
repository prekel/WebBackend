import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import RootComponent from './home.jsx';

import {Cart} from '../fsharp/Cart.js'
import {Carts} from '../fsharp/Carts.js'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.Components = {RootComponent, Cart, Carts};
