import ApplicationAdapter from './application';
import DS from 'ember-data';
import DataAdapterMixin from 'ember-simple-auth/mixins/data-adapter-mixin';

export default DS.RESTAdapter.extend(DataAdapterMixin,{
	authorizer: 'authorizer:oauth2',
	host: 'http://localhost:60752',
	namespace: 'api',
	buildURL: function(type, id, record) {
    	return this.host+"/"+this.namespace+'/account/userinfo'
  	}
});
