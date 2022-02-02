const { createYield } = require("typescript")

describe('Home Page (list-all-policy)', () => {
  beforeEach(() => {

    cy.fixture('policy-details.json').as("polictDetailsJSON");

    cy.server();

    cy.route('https://localhost:44330/api/v1.0/Policy/GetAll', "@polictDetailsJSON").as("mockPolicyDetails");

    cy.visit('/');

  });

  it('should list all policies', () => {
    cy.get('.text-decoration-underline').contains('List All Policies');
    cy.wait("@mockPolicyDetails");
    cy.get('table > tbody > tr').should("have.length", 4);
    cy.contains('table > tbody > tr:nth-child(4) > td:nth-child(6)', 'Test Company #4');
  })
})
