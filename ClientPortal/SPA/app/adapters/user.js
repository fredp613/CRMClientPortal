import DS from 'ember-data';
import DataAdapterMixin from 'ember-simple-auth/mixins/data-adapter-mixin';


export default DS.RESTAdapter.extend({
	authorizer: 'authorizer:oauth2',
	host: 'http://localhost:60752',
	namespace: 'api/Account/UserInfo',
	findRecord: function(store, type, id) {
    	//id = "foo" + id;

    	console.log("finding1")
    	return this._super("account", "userinfo");
  	},	
  	findQuery: function(store, type, id) {
    	//id = "foo" + id;
    	console.log("finding")
    	return this._super("account", "userinfo");
  	},
	// updateRecord(store, type, snapshot) {
 //    	const url = `${this.host}/api/account/userinfo;
 //    	return this.ajax(url, "PUT", {});
 //  	}

});
