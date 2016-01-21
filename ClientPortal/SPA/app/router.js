import Ember from 'ember';
import config from './config/environment';

const Router = Ember.Router.extend({
  location: config.locationType
});

Router.map(function() {
  this.resource('forms');
  this.route('login');
  this.route('protected');
  this.route('users');
  this.route('register');
  this.route('home');

  this.route('Account', function() {
    this.route('UserInfo');
  });
  this.route('account');
});

export default Router;
