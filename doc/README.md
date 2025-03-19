# OrderProcessor

## Tech stack
- **IDE** - Visual Studio 2022 Community v17.13.3
- **Database engine** - SQlite v3.13.0
- **External libraries**
    - Microsoft.EntityFrameworkCore v9.0.3
    - Microsoft.EntityFrameworkCore.Sqlite v9.0.3

## Final thoughts & Future Development
Due to time and experience limitations, this project is considered as a prototype. Below are some thoughts for potential future improvements.

### 1. General Application Architecture
- **Business Logic Implementation:**  
    Currently, input validation is implemented to handle all invalid inputs from the user. It may be easier eventually to remove this logic rather than rearchitect the system around potentially error-prone inputs.
- **Central Library Directory:**  
    Since most components use the same libraries, I would store all necessary libraries in a single directory.

### 2. BO Layer
- **Current Order entity sructure:**  
    As it was described in task requirements that Order should consit **at least** of value, product name, client type, shipping address and payment method I've extened it with quantity, creation time, status and customer name.
- **Customers Table:**  
    For future enhancements, storing customer data in a separate table could be beneficial for additional system functionality.
- **Potential future Order entity sructure:**
    In caase where I store customers data in spearate table, I'd remove properties like customer name, customer type from `Order.cs` and replace it with ID of customer.

### 3. Service Layer
- **ORM Choice:**  
    In a real business implementation, using a different ORM such as Dapper could be preferable over the currently employed EntityFramework.
- **Table Printing:**  
    The current table printing function is static. A future improvement would involve making this functionality generic for better maintainability.
- **Additional Functionalities:**  
    Functionalities like editing and deleting orders have been added, which are essential for a system processing external requests.

### 4. Tests
- **TDD Technique:**  
    Although I've learned about Test-Driven Development (TDD) during this process which in future development would be beneficial for better project organization. For this prototype I implemented simple unit tests.
- **Manual Testing Requirement:**  
    The `OrderManagmentService.cs` class currently requires manual testing due to logic within methods like `GetValidOrderStatus()` and `CreateOrderDetails()` that handle user interaction in the console.