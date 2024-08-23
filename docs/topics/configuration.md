# Configuration


## Retrieving a Configuration

1. **Navigate to the UI:**
   - Open your web browser and go to the %product% UI.

2. **Create a New Project:**
   - Click on the **Projects** --> **Projects** in menu.
   - Browse or search the project you want retrieve the configurations.
   - Click on the project name. Example `Project 1`

   ![Retrieve Configuration](retrieve_configuration.png){width="600"}{border-effect=line}

## Adding a Configuration

1. **Navigate to the UI:**
   - Open your web browser and go to the %product% UI.

2. **Create a New Project:**
   - Click on the **Projects** --> **Projects** in menu.
   - Browse or search the project you want retrieve the configurations.
   - Click on the project name. Example `Project 1`
   - Click ![Add Icon](add_icon_filled.svg){style="inline"}.
   - Enter the **Key**, **Value**, and select the **Type** of the configuration. The type check ensures that the data you input matches the expected format.
   - Click **Create** to add the configuration to the database.

   *Example:*
    - **Key:** `ConnectionStrings:DefaultConnection`
    - **Value:** `Host=localhost;Database=my_db;Username=user;Password=pass`
    - **Type:** `String`

   ![Adding Configuration Example](create_configuration.png){width="600"}{border-effect=line}

##### Updating a Configuration

1. **Navigate to the UI:**
   - Open your web browser and go to the %product% UI.

2. **Create a New Project:**
   - Click on the **Projects** --> **Projects** in menu.
   - Browse or search the project you want retrieve the configurations.
   - Click on the project name. Example `Project 1`.
   - Click ![Edit Icon](edit_icon_outlined.svg){style="inline"}.
   - Enter new values name
   - Click ![Accept Icon](checked_icon_outlined.svg) button update configuration.

   *Example:*
    - Updating the database connection string if the database credentials change.

   ![Editing Configuration Example](edit_configuration.png){width="600"}{border-effect=line}


##### Deleting a Configuration

1. **Navigate to the UI:**
   - Open your web browser and go to the %product% UI.

2. **Create a New Project:**
   - Click on the **Projects** --> **Projects** in menu.
   - Browse or search the project you want retrieve the configurations.
   - Click on the project name. Example `Project 1`.
   - Click ![Delete Icon](delete_icon_outlined.svg){style="inline"}.

   *Example:*
    - Removing an obsolete configuration that is no longer needed by your application.