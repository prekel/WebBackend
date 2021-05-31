import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout.jsx';
import { Home } from './components/Home.jsx';
import { FetchData } from './components/FetchData.jsx';
import { Counter } from './components/Counter.jsx';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute.jsx';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes.jsx';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants.jsx';

//import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
