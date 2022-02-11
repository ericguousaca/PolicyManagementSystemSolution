const { createYield } = require("typescript")

describe('Home Page (list-all-policy)', () => {
  beforeEach(() => {
      //cy.intercept({ method: 'GET', url: '**/GetAll*', resultData }).as("mockPolicyDetailGridDataResult");
      cy.viewport(1600, 720)
  });

  it('should list all policies', () => {
    cy.fixture('policyDetailGridDataResult.json').then(resultData => {
      console.log(resultData)
      cy.server()
      cy.intercept('GET', 'https://localhost:44330/api/v1.0/Policy/GetAll?skip=0&pageSize=5&sortBy=id&sortDirection=asc', resultData).as("mockPolicyDetailGridDataResult");

      cy.visit('/');

      cy.wait("@mockPolicyDetailGridDataResult");

      cy.get('.text-decoration-underline').contains('List All Policies');
      cy.get('table > tbody > tr').should("have.length", 5);
      cy.contains('table > tbody > tr:nth-child(4) > td:nth-child(6)', 'Test Company #4');
    });
  })
})
