import Ember from 'ember';
import User from '../models/user'

export default Ember.Controller.extend({
  session: Ember.inject.service('session'),
  actions: {
    authenticate() {
      let { identification, password } = this.getProperties('identification', 'password');

      var existingUser = User.create({
      	session: this.get('session'),
      	email: identification,
      	password: password
      });
      existingUser.login();

      // this.get('session').authenticate('authenticator:oauth2', identification, password).catch((reason) => {
      //   this.set('errorMessage', reason.error || reason);
      // });
    }
  }
});