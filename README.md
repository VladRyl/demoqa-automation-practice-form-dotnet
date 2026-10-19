# DemoQA browser tests

## Run with Docker

The container uses the Playwright .NET image matching the `Microsoft.Playwright` package version in the test project.

```bash
docker build -t demoqa-automation-practice-form .
docker run --rm --init --ipc=host \
  -v "$PWD/allure-results:/src/allure-results" \
  demoqa-automation-practice-form
```

Test results are written to the host's `allure-results` directory. To build and open the HTML report locally, install the Allure CLI and run:

```bash
allure generate allure-results --output allure-report
allure open allure-report
```

## GitHub Actions

The `UI tests and Allure report` workflow runs on pushes, pull requests, and manual dispatches. Each run uploads two artifacts:

- `allure-results` — raw test data
- `allure-report` — generated HTML report

Download either artifact from the completed workflow run in the repository's **Actions** tab. The report is generated even when tests fail, so failures remain inspectable.
