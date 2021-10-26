#r "paket:
nuget Fake.Api.GitHub
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"

open System
open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Api

Target.initEnvironment ()

Target.create "Upload" (fun _ ->
    // you may need to delete build.fsx.lock and remove fake-cli tool, add new version and then
    // restore packages again.
    let user = "yazeedobaid"
    let repository = "FakeGitHubReleaseAPI"
    let productVersion = "v1.0.0"
    let prerelease = false
    let notes = seq { "Release from FAKE's GitHub API module" }
    let files = !! ("./files/" + "*.txt" )

    "your-GitHub-pesonal-access-token"
    |> GitHub.createClientWithToken
    |> GitHub.draftNewRelease user repository productVersion prerelease notes
    |> GitHub.uploadFiles files
    |> GitHub.publishDraft
    |> Async.RunSynchronously
)


Target.create "DryRun" ignore

"Upload"
  ==> "DryRun"

Target.runOrDefault "DryRun"