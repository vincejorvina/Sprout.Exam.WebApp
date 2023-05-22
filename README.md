Possible new features:

1. Salary (for regular), Day Rate (for contractual), Tax Deductions, etc. can be added into the database, in order to simplify the synchronization between the front-end and back-end. Currently the amounts are hardcoded on the source code, meaning changes would have to be done separately on the back-end and front-end if ever there is a change on the values. By putting them on the database instead, very little work would have to be done if the values were to be updated.
2. Input validation could possibly done without the need for pop-up dialog boxes for a better user experience. For example, the submit button would be disabled if there is an invalid input on a field, with the specific field being highlighted, along with a tooltip showing what makes it invalid.
