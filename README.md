# DemoQA browser tests

## Run with Docker

The container uses the Playwright .NET image matching the `Microsoft.Playwright` package version in the test project.

```bash
docker compose run --build --rm ui-tests
```

Test results are written to the host's `allure-results` directory. To build and open the HTML report locally, install the Allure CLI and run:

```bash
allure generate allure-results --output allure-report
allure open allure-report
```

## GitHub Actions

The `UI tests and Allure report` workflow uses Docker Compose to build and run the tests on GitHub Actions for pushes, pull requests, and manual dispatches. Each run uploads two artifacts:

- `allure-results` — raw test data
- `allure-report` — generated HTML report

Download either artifact from the completed workflow run in the repository's **Actions** tab. The report is generated even when tests fail, so failures remain inspectable.
