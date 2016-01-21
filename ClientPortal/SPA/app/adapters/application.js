import DS from 'ember-data';
import DataAdapterMixin from 'ember-simple-auth/mixins/data-adapter-mixin';

export default DS.RESTAdapter.extend({
	authorizer: 'authorizer:oauth2',
	host: 'http://localhost:60752',
	namespace: 'api'

});

// namespace: 'api',
//   authorizer: 'authorizer:application'



