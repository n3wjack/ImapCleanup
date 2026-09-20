FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

COPY . /source
WORKDIR /source/ImapCleanup

ARG TARGETARCH

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -p:PublishSingleFile=true -a ${TARGETARCH/amd64/x64} --use-current-runtime \
    --self-contained true -p:PublishTrimmed=true -p:TrimMode=partial -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-alpine AS final
WORKDIR /app

# Copy everything needed to run the app from the "build" stage.
COPY --from=build /app .
USER $APP_UID

ENTRYPOINT ["./ImapCleanup"]

