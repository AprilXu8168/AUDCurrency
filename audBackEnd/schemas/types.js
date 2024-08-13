const graphql = require("graphql");
const { GraphQLObjectType, GraphQLString, GraphQLInt } = graphql;

const CurrencyType = new GraphQLObjectType({
  name: "Currency",
  type: "Query",
  fields: {
    id: { type: GraphQLInt },
    timeStamp: { type: GraphQLInt },
    name: { type: GraphQLString },
    moneyCode: { type: GraphQLString },
    baseValue: { type: GraphQLString },
    value: {type: GraphQLString}
  }
});

exports.CurrencyType = CurrencyType;