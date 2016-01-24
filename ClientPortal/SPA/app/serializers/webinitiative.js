import DS from 'ember-data';
import Ember from 'ember';

export default DS.RESTSerializer.extend({
	// normalizeResponse: function(store, primaryType, payload, id, requestType) {
	//      var i, record, payloadWithRoot;
	//     // // if the payload has a length property, then we know it's an array
	//     // if (payload.length) {
	//     //   for (i = 0; i < payload.length; i++) {
	//     //     record = payload[i];
	//     //     this.mapRecord(record);
	//     //   }
	//     // } else {
	//     //   // payload is a single object instead of an array
	//     //   this.mapRecord(payload);
	//     // }
	//     payloadWithRoot = {}
	//     payloadWithRoot[primaryType.typeKey] = payload
	//     this._super(store, primaryType, payloadWithRoot, id, requestType);
	// }
});
